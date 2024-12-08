function setDeleteModalData(button) {
    let divDel = document.getElementById("modal-delete-body");
    divDel.textContent = `¿Desea eliminar a ${button.dataset.name} de la base de datos?`;
    let buttonDel = document.getElementById("modal-delete-id");
    buttonDel.setAttribute("value", button.dataset.id);
}

function setAddTaskExceptData(button) {
    let textInput = document.getElementById("text-filter-task");

    let submitBtn = document.getElementById("submit-exception");
    let inputTaskID = document.getElementById("add-except-task-id")
    let inputPersonID = document.getElementById("add-except-person-id")

    inputTaskID.setAttribute("value", "none");
    submitBtn.setAttribute("class", "btn btn-lg btn-outline-success");
    submitBtn.setAttribute("disabled", "true");
    
    let id = button.dataset.id;
    inputPersonID.setAttribute("value", id)

    let dbox = document.getElementById("task-dropbox-menu");

    Array.from(dbox.children).forEach( child => {
        dbox.removeChild(child)
    })

    //petición a servidor 
    //-------------------v Handler (como asp-page-handler)
    fetch("/Persons/AvailTasks", {
        method: "POST",
        headers: {
            'RequestVerificationToken': document.getElementsByName("__RequestVerificationToken")[0].value,
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ data: [id] })
    })
    .then(response => response.json())
    .then(r => {
        if(r.success){
            r.data.forEach( task => {
                let li = document.createElement("li");
                let button = document.createElement("button");
                button.textContent = task.name;
                button.setAttribute("class", "dropdown-item");
                button.setAttribute("type", "button");
                button.dataset.id = task.id;
                
                button.onclick = function () {
                    let textInput = document.getElementById("text-filter-task");
                    textInput.value = this.textContent
                    let submit = document.getElementById("submit-exception");
                    submit.setAttribute("class", "btn btn-lg btn-success");
                    submit.removeAttribute("disabled");

                    let inputTaskId = document.getElementById("add-except-task-id")
                    inputTaskId.setAttribute("value", this.dataset.id);
                };

                li.appendChild(button);
                dbox.appendChild(li)
            })
        }
    })
}

//función para listar tareas disponibles
function displayAvailTasks(textInput){

    let submitBtn = document.getElementById("submit-exception");
    let inputTaskID = document.getElementById("add-except-task-id")
    inputTaskID.setAttribute("value", "none")
    submitBtn.setAttribute("class", "btn btn-lg btn-outline-success");
    submitBtn.setAttribute("disabled", "true");

    let dbox = document.getElementById("task-dropbox-menu");
    let dboxOptions = dbox.children
    let dboxDiv = document.getElementById("dropdown-div");
    let filter = textInput.value.trim().toLowerCase();

    let triggerButton = document.createElement("button")
    triggerButton.style.display = "none";

    triggerButton.setAttribute("class", "btn btn-secondary dropdown-toggle")
    triggerButton.setAttribute("type", "button");
    triggerButton.setAttribute("aria-expanded", "false");
    triggerButton.setAttribute("data-bs-toggle", "dropdown");
    triggerButton.setAttribute("data-bs-auto-close", "true");
    dboxDiv.appendChild(triggerButton);
    if (dbox.style.display == "none")
        triggerButton.click();

    Array.from(dboxOptions).forEach( task => {
        
        task.style.display = ((task.textContent.toLowerCase().includes(filter))) ?
            "block" : "none"
    })
}

(function () {
    'use strict'
  
    var forms = document.querySelectorAll('.needs-validation')
  
    Array.prototype.slice.call(forms)
      .forEach(function (form) {
        form.addEventListener('submit', function (event) {
          if (!form.checkValidity()) {
            event.preventDefault()
            event.stopPropagation()
          }
  
          form.classList.add('was-validated')
        }, false)
      })
  })()