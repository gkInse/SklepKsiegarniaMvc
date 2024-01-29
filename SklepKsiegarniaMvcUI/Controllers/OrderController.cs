using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SklepKsiegarniaMvcUI.Data;
using SklepKsiegarniaMvcUI.Models;

namespace SklepKsiegarniaMvcUI.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Order
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Orders.Include(o => o.OrderStatus);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Order/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderStatus)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Order/Create
        public IActionResult Create()
        {
            ViewData["OrderStatusId"] = new SelectList(_context.orderStatuses, "Id", "StatusName");
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,CreateDate,OrderStatusId,IsDeleted")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderStatusId"] = new SelectList(_context.orderStatuses, "Id", "StatusName", order.OrderStatusId);
            return View(order);
        }

        // GET: Order/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderDetail)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["OrderStatusId"] = new SelectList(_context.orderStatuses, "Id", "StatusName", order.OrderStatusId);
            return View(order);
        }


        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }
            if (order != null)
            {
                _context.Update(order);
            }
            await _context.SaveChangesAsync();

            // Dodaj produkty z zamówienia do listy najczęściej kupowanych produktów
            if (order.OrderStatusId == 3) // Przyjmując, że Statuses.Delivered to status dostarczony
            {
                order.OrderDetail = await _context.OrderDetails.Where(od => od.OrderId == order.Id).ToListAsync();
                foreach (var orderDetail in order.OrderDetail)
                {
                    var topProduct = await _context.TopProducts.FirstOrDefaultAsync(tp => tp.ProductId == orderDetail.BookId);
                    if (topProduct != null)
                    {
                        topProduct.Quantity += orderDetail.Quantity;
                    }
                    else
                    {
                        _context.TopProducts.Add(new TopProducts { ProductId = orderDetail.BookId, Quantity = orderDetail.Quantity });
                    }
                }
                await _context.SaveChangesAsync();
            }
            ViewData["OrderStatusId"] = new SelectList(_context.orderStatuses, "Id", "StatusName", order.OrderStatusId);
            return View(order);

            //if (ModelState.IsValid)
            //{
            //    try
            //    {
            //        _context.Update(order);
            //        await _context.SaveChangesAsync();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        if (!OrderExists(order.Id))
            //        {
            //            return NotFound();
            //        }
            //        else
            //        {
            //            throw;
            //        }
            //    }
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["OrderStatusId"] = new SelectList(_context.orderStatuses, "Id", "StatusName", order.OrderStatusId);
            //return View(order);
        }

        public async Task<IActionResult> TopProducts()
        {
            var topProducts = await _context.TopProducts
                                            .Include(tp => tp.Product) // Ładuje dane książek
                                            .OrderByDescending(tp => tp.Quantity)
                                            .Take(10)
                                            .ToListAsync();

            return View(topProducts);
        }



        // GET: Order/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.OrderStatus)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return (_context.Orders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
