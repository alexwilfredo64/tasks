using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Tasks.EntityModels;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace Tasks.Web.Pages;

public class PersonsModel : PageModel
{
    public class RequestData {
        public required List<string> data { get; set; }
    }
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
        Persons = _db.Persons.Include(p => p.ActivityDetails).OrderBy(x => x.FirstName + x.LastName);
    }

    public IActionResult OnPostAddPerson()
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

    public IActionResult OnPostDeletePerson(int id)
    {
        Person p = _db.Persons.Single(p => p.PersonID == id);
        _db.Persons.Remove(p);
        _db.SaveChanges();
        
        return RedirectToPage("/Persons");
    }

    public IActionResult OnPostAvailTasks([FromBody] RequestData Data)
    {
        if(!int.TryParse(Data.data[0], out int TaskId)){
            WriteLine("[ERROR]: No se pudo convetir '{1}' a int", Data.data[0]);
            return new JsonResult( new {success = false, data = ""} );
        }

        IEnumerable<TaskException> UsedTasks = _db.TaskExceptions.Where(te => te.PersonID == TaskId);

        List<int> UsedTasksIds = new();

        foreach(TaskException te in UsedTasks){ UsedTasksIds.Add(te.TaskID); }

        IEnumerable<Tasks.EntityModels.Task> AvailTasks = _db.Tasks.Where(t => !UsedTasksIds.Contains(t.TaskID)).OrderBy(t => t.TaskName);

         List<Object> nameIdPairs = new();

         foreach(var t in AvailTasks)
         {
            nameIdPairs.Add(new { name = t.TaskName, id = t.TaskID});
         }

         return new JsonResult( new { success = true, data = nameIdPairs });
    }

    public IActionResult OnPostAddException(int PersonID, int TaskID)
    {
             
        TaskException te = new()
        {
            TaskID = TaskID,
            PersonID = PersonID
        };

        _db.TaskExceptions.Add(te);
        _db.SaveChanges();
        
        return RedirectToPage("/Persons");
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