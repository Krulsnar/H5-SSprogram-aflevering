using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using H5_SSP_aflevering.Models;
using H5_SSP_aflevering.Code;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authorization;
using H5_SSP_aflevering.Areas.Identity.Code;

namespace H5_SSP_aflevering.Controllers
{
    [Authorize("RequireAuthenticatedUser")]
    public class TodoesController : Controller
    {
        private readonly H5_SSP_TODOContext _context;
        private readonly Data.ApplicationDbContext _applicationDbContext;
        private readonly IDataProtector _dataProtector;
        private readonly Encryption _encryption;

        public TodoesController(
            H5_SSP_TODOContext context, 
            Data.ApplicationDbContext applicationDbContext,
            IDataProtectionProvider dataProtector,
            Encryption encryption
            )
        {
            _applicationDbContext = applicationDbContext;
            _context = context;
            _dataProtector = dataProtector.CreateProtector("testKey");
            _encryption = encryption;
        }

        // GET: Todoes
        public async Task<IActionResult> Index()
        {
            var userName = User.Identity.Name;
            List<Todo> todolist;

            if (User.IsInRole("Admin"))
            {
                todolist = await _context.Todos.OrderBy(x => x.UserId).ToListAsync();

                foreach (var todo in todolist)
                {
                    todo.Note = _encryption.Decrypt(todo.Note, _dataProtector);
                }
            }
            else
            {
                todolist = await _context.Todos.Where(user => user.UserId == userName).ToListAsync();

                foreach (var todo in todolist)
                {
                    todo.Note = _encryption.Decrypt(todo.Note, _dataProtector);
                }
            }

            return View(todolist);
        }

        // GET: Todoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }
            ViewData["noteEncrypted"] = todo.Note;
            todo.Note = _encryption.Decrypt(todo.Note, _dataProtector);

            return View(todo);
        }

        // GET: Todoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Todoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Title,Note")] Todo todo)
        {
            if (ModelState.IsValid)
            {
                todo.UserId = User.Identity.Name;
                todo.Note = _encryption.Encrypt(todo.Note, _dataProtector);
                
                _context.Add(todo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // GET: Todoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todos.FindAsync(id);
            
            if (todo == null)
            {
                return NotFound();
            }
            todo.Note = _encryption.Decrypt(todo.Note, _dataProtector);
            return View(todo);
        }

        // POST: Todoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Title,Note")] Todo todo)
        {
            if (id != todo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    todo.UserId = User.Identity.Name;
                    todo.Note = _encryption.Encrypt(todo.Note, _dataProtector);
                    _context.Update(todo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoExists(todo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(todo);
        }

        // GET: Todoes/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var todo = await _context.Todos
                .FirstOrDefaultAsync(m => m.Id == id);
            todo.Note = _encryption.Decrypt(todo.Note, _dataProtector);
            if (todo == null)
            {
                return NotFound();
            }

            return View(todo);
        }

        // POST: Todoes/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TodoExists(int id)
        {
            return _context.Todos.Any(e => e.Id == id);
        }
    }
}
