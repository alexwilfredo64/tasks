using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Tasks.EntityModels;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
namespace Tasks.Web.Pages;
public class ActivityDetailsModel : PageModel
{
    private TasksContext _db;


    public ActivityDetailsModel(TasksContext db)
    {
        _db = db;
    }

    public IEnumerable<ActivityDetail>? ActivityDetails { get; set; }

    public ActivityDetail ActivityDetail { get; set; } = new();

    public List<string>? Suggestions { get; set; } = new();

    public IEnumerable<EntityModels.Task> tasks { get; set; }
    public EntityModels.Task task { get; set; }

    [BindProperty]
    public string SearchBox { get; set; }

    public void OnGet(int id)
    {

        List<int>? usedTasks = new();
        ActivityDetails = _db.ActivityDetails.Where(a => a.ActivityID == id).Include(a => a.Activity).Include(a => a.Person).Include(a => a.Task).OrderBy(a => a.Index);

        TempData["id"] = id;
        TempData["n"] = ActivityDetails.Count();
        

        foreach (var a in ActivityDetails)
        {
            usedTasks.Add(a.TaskID);
        }

        tasks = _db.Tasks.Where(a => !usedTasks.Contains(a.TaskID));
        TempData["usedTasks"]=usedTasks;

        foreach (var a in tasks)
        {
            Suggestions!.Add(a.TaskName);
        }
        WriteLine("OnGet()");



    }
   
    public IActionResult OnPostDetails(int ActivityId)
    {
        return RedirectToPage("/ActivityDetails", new { id = ActivityId });
    }
    public IActionResult OnPostDelete(int index)
    {
        int Id = Convert.ToInt32(TempData["id"]);
        ActivityDetails = _db.ActivityDetails.Where(a => a.ActivityID == Id).Include(a => a.Activity).Include(a => a.Person).Include(a => a.Task).OrderBy(a => a.Index);
        foreach (var a in ActivityDetails)
        {
            if (a.Index > index)
            {
                WriteLine("xd");
                a.Index--;
                _db.ActivityDetails.Update(a);
            }
            else if (a.Index == index)
            {
                WriteLine("xd1");
                _db.ActivityDetails.Remove(a);
            }
        }


        _db.SaveChanges();
        return RedirectToPage("/ActivityDetails", new { id = Id });
    }

    public IActionResult OnPostAdd()
    {
        //var usedTasks=JsonConvert.DeserializeObject<List<int>>(TempData["usedTasks"].ToString());
        int Id = Convert.ToInt32(TempData["id"]);
        if (ModelState.IsValid)
        {
            int index = Convert.ToInt32(TempData["n"]);
            WriteLine(SearchBox);
            task = _db.Tasks.FirstOrDefault(a => a.TaskName == SearchBox)!;
            if (task != null)
            {
                ActivityDetail.ActivityID = Id;
                ActivityDetail.TaskID = task.TaskID;
                ActivityDetail.Index = index + 1;
                _db.Add(ActivityDetail);
                _db.SaveChanges();

                return RedirectToPage("/ActivityDetails", new { id = Id });
            }

        }
        return BadRequest();

    }

}




