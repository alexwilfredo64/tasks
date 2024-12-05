using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Tasks.EntityModels;
using Microsoft.EntityFrameworkCore;

namespace Tasks.Web.Pages;

public class PersonsModel : PageModel
{
    private TasksContext _db;

    public PersonsModel(TasksContext db)
    {
        _db = db;
    }

    public IEnumerable<Person>? Persons { set; get; }

    [BindProperty]
    public Person? Person { set; get; }

    public void OnGet() 
    {
        ViewData["title"] = "Personas";
        Persons = _db.Persons.OrderBy(x => (x.FirstName + x.LastName));
    }

    public IActionResult OnPostAdd()
    {
        if(Person is not null && ModelState.IsValid )
        {
            _db.Persons.Add(Person);
            _db.SaveChanges();
            return RedirectToPage("/Persons");
        } else {
            return BadRequest();
        }
    }

    public IActionResult OnPostDelete()
    {
        if(Person is not null && ModelState.IsValid )
        {
            return RedirectToPage("/Persons");
        } else {
            return BadRequest();
        }
    }
    public string getPersonExceptionTasksNames(int id)
    {

        IQueryable<TaskException> TaskExceptions = _db.TaskExceptions.Include(te => te.Task).Where( te => te.PersonID == id);
        string tasksNames = "";
        foreach(TaskException te in TaskExceptions)
        {
            tasksNames += te.Task.TaskName + ", "; 
        }

        if(String.IsNullOrEmpty(tasksNames))
            return "";
        
        return tasksNames.Substring(0, tasksNames.Length - 2);
    }
}