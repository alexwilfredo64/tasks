using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Tasks.EntityModels;

namespace Tasks.Web.Pages;
public class ActivitiesModel : PageModel
{
    private TasksContext _db;
    public ActivitiesModel(TasksContext db)
    {
        _db = db;
        
    }

     private void SetViewData()
    {
        ViewData["title"] = "Actividades";
    }

    

    public IEnumerable<Activity>? Activities { get; set; }
    
    
    [BindProperty]
    public int Id { get; set;}


    [BindProperty]
    public Activity Activity { get; set; }


    [BindProperty]
    public DateTime fecha { get; set; } = DateTime.MaxValue;


    public void OnGet()
    {
       SetViewData();
        Activities = _db.Activities.OrderByDescending(a => a.Date);
    }

public void OnPost(){
    
}

    public IActionResult OnPostDelete()
    {
       SetViewData();
        Activity = _db.Activities.First(a => a.ActivityID == Id);
        _db.Activities.Remove(Activity);
        _db.SaveChanges();
        return RedirectToPage("/Activities");
    }


    public IActionResult OnPostAdd()
    {
        SetViewData();
        if(ModelState.IsValid){
        _db.Activities.Add(Activity);
        _db.SaveChanges();
        return RedirectToPage("/Activities");
        }
         return BadRequest ();

    }
    public IActionResult OnPostFilterDate()
    {
         SetViewData();
        Activities = _db.Activities.Where(a => a.Date < fecha).OrderByDescending(a => a.Date);
        return Page();
    }
}