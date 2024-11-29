using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Tasks.EntityModels;
namespace Tasks.Web.Pages;
public class ActivitiesModel : PageModel{
private TasksContext _db;
public ActivitiesModel(TasksContext db){
    _db = db;
}
public IEnumerable<Activity>? Activities{get; set;}

[BindProperty]
public Activity Activity{get; set;}
[BindProperty]
public DateTime? fecha{get;set;}=null;
public void  OnGet(){
   
if(fecha is not null){
    WriteLine(fecha);
    Activities = _db.Activities;
    Activities=Activities.Where(a=>a.Date < fecha).OrderByDescending(a=>a.Date);

}
else{
Activities = _db.Activities;
Activities=Activities.OrderByDescending(a=>a.Date);
}
}
public IActionResult OnPost(){


     if(Activity is not null && ModelState.IsValid){
         _db.Activities.Add(Activity);
         _db.SaveChanges();
         return RedirectToPage("/Activities");
      }
        
        OnGet();

        // Aquí se maneja la fecha seleccionada
        return Page();
        
       

        //return RedirectToPage("/Activities"); // Devuelve la misma página o redirige según la lógica
        
       
}

}