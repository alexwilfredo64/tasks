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
    public IEnumerable<Activity>? Activities { get; set; }

    [BindProperty]
    public Activity Activity { get; set; }
    [BindProperty]
    public DateTime fecha { get; set; } = DateTime.MaxValue;
    public void OnGet()
    {
        Activities = _db.Activities.OrderByDescending(a => a.Date);
    }



    public IActionResult OnPostDelete(int id)
    {
        Activity = _db.Activities.First(a => a.ActivityID == id);
        _db.Activities.Remove(Activity);
        _db.SaveChanges();
        return RedirectToPage("/Activities");
    }
    public IActionResult OnPostAdd()
    {
        if(ModelState.IsValid){
        _db.Activities.Add(Activity);
        _db.SaveChanges();
        return RedirectToPage("/Activities");
        }
         return BadRequest ();

    }
    public IActionResult OnPostFilterDate()
    {
        Activities = _db.Activities.Where(a => a.Date < fecha).OrderByDescending(a => a.Date);
        return Page();
    }
}