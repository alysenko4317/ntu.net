using Lab6.Models;
using Lab6.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab6.Controllers {

[Route("api/[controller]")]
[ApiController]
public class WorkersController : ControllerBase
{
    private IBaseRepository<Worker> Workers { get; set; }

    public WorkersController(IBaseRepository<Worker> workers) {
        Workers = workers;
    }

    [HttpGet]   // GET: api/Workers
    public ActionResult<IEnumerable<Worker>> GetWorkers() {
        return Workers.GetAll();
    }

    [HttpPost]   // POST: api/Workers
    public ActionResult<Worker> PostWorker(Worker worker) {
        var createdWorker = Workers.Create(worker);
        return CreatedAtAction(nameof(GetWorker), new { id = createdWorker.Id }, createdWorker);
    }

    [HttpDelete("{id}")]   // DELETE: api/Workers/{id}
    public ActionResult<Worker> DeleteWorker(Guid id) {
        var worker = Workers.Get(id);
        if (worker == null)
            return NotFound();
        Workers.Delete(id);
        return worker;
    }

    [HttpGet("{id}")]   // GET: api/Workers/{id}
    public ActionResult<Worker> GetWorker(Guid id) {
        var worker = Workers.Get(id);
        if (worker == null)
            return NotFound();
        return worker;
    }
}
}
