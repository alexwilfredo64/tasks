using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Tasks.EntityModels;

namespace Tasks.Web.Pages;

public class IndexModel : PageModel
{

  private TasksContext _db { get; set; }

  public IndexModel(TasksContext db)
  {
    _db = db;
  }

  public IEnumerable<Activity>? Activities { get; set; }

  [BindProperty]
  public Activity? Activity { get; set; }

  public void OnGet()
  {
    DateTime todayDate = DateTime.Now.Date;
    DateTime oneMonthLaterDate = todayDate.AddMonths(1);

    ViewData["title"] = "Inicio";
    Activities = _db.Activities.Where(a => (a.Date >= todayDate) && (a.Date <= oneMonthLaterDate)).OrderBy(a => a.Date);
  }

}