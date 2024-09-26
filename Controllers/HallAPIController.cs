using System.Xml.Linq;
using BanquetHallProject.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using BanquetHallProject.Data;

//  dotnet ef migrations add InitialCreate
//  dotnet ef database update


namespace BanquetHallProject.Controllers
{
    [ApiController]
    [Route("/booking")]
    public class HallAPIController : ControllerBase
    {
        private readonly HallDbContext _context;

        public HallAPIController(HallDbContext context)
        {
            _context = context;
        }

        //GET 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HallAPIDataModel>>> GetBookings()
        {
            return await _context.HallBookings.ToListAsync();
        } 


        //GET by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<HallAPIDataModel>> GetBooking(int id)
        {
            var booking = await _context.HallBookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        //POST
        [HttpPost]
        public async Task<ActionResult<HallAPIDataModel>> CreateBooking(HallAPIDataModel newBooking)
        {
            _context.HallBookings.Add(newBooking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBooking), new { id = newBooking.Id }, newBooking);
        }


        private bool BookingExists(int id)
        {
            return _context.HallBookings.Any(e => e.Id == id);
        }

        //PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, HallAPIDataModel updatedBooking)
        {
            if (id != updatedBooking.Id)
            {
                return BadRequest();
            }

            _context.Entry(updatedBooking).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookingExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //PATCH
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchBooking(int id, HallAPIDataModel updatedBooking)
        {
            var booking = await _context.HallBookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            // Update only provided fields
            if (!string.IsNullOrEmpty(updatedBooking.Name))
            {
                booking.Name = updatedBooking.Name;
            }

            if (updatedBooking.Capacity > 0)
            {
                booking.Capacity = updatedBooking.Capacity;
            }

            if (updatedBooking.Price > 0)
            {
                booking.Price = updatedBooking.Price;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }

        //DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.HallBookings.FindAsync(id);

            if (booking == null)
            {
                return NotFound();
            }

            _context.HallBookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}


//---------------------------------------------Goes inside your class -----------------------------------------------------
//HARD CODING DATA AND PERFORMING CRUD API
//METHOD 1
/*
[HttpGet]
public IActionResult GetBookings()
{
    var bookings = new List<HallAPIDataModel>
    {
         new HallAPIDataModel
         {
            Id = 101,
            Name = "Banyan Room",
            Capacity = 25,
            Price = 20000
         },
        new HallAPIDataModel
        {
            Id = 102,
            Name = "Chinar Room",
            Capacity = 50,
            Price = 30000
        },
        new HallAPIDataModel
        {
            Id = 103,
            Name = "Ashoka Room",
            Capacity = 30,
            Price = 26000
        },
        new HallAPIDataModel
        {
            Id = 104,
            Name = "Mango Room",
            Capacity = 10,
            Price = 15000
        }
    };
    return Ok(bookings);
}

[HttpPost]
public IActionResult CreateBookings([FromBody] HallAPIDataModel mydata)
{
    var bookings = new List<HallAPIDataModel>
    {
         new HallAPIDataModel
         {
            Id = 101,
            Name = "Banyan Room",
            Capacity = 25,
            Price = 20000
         },
        new HallAPIDataModel
        {
            Id = 102,
            Name = "Chinar Room",
            Capacity = 50,
            Price = 30000
        },
        new HallAPIDataModel
        {
            Id = 103,
            Name = "Ashoka Room",
            Capacity = 30,
            Price = 26000
        },
        new HallAPIDataModel
        {
            Id = 104,
            Name = "Mango Room",
            Capacity = 10,
            Price = 15000
        }
    };
    var LastID = bookings.Last();

    mydata.Id = LastID.Id + 1;

    bookings.Add(mydata);

    return Ok(bookings);
}


[Route("/{id}&{eventName}")]
[HttpPut]
public IActionResult UpdateBookingUsingPut(int Id, string Name)
{
    var bookings = new List<HallAPIDataModel>
    {
        new HallAPIDataModel
        {
                Id = 104,
                Name = "Mango Room",
                Capacity = 10,
                Price = 15000
        },
        new HallAPIDataModel
        {
                Id = 103,
                Name = "Ashoka Room",
                Capacity = 30,
                Price = 26000                
        },
        new HallAPIDataModel
        {
                Id = 102,
                Name = "Chinar Room",
                Capacity = 50,
                Price = 30000                
        }
    };
    if (Id == 0 || bookings.Count() > Id)
    {
        return BadRequest("Operation not performed");
    }
    var i = from l in bookings where l.Id == Id select l;
    i.First().Name = Name;
    return Ok(bookings);
}

[Route("/{id}&{eventName}")]
[HttpPatch]
public IActionResult UpdateBookingUsingPatch(int Id, string Name)
{
    var bookings = new List<HallAPIDataModel>
    {
        new HallAPIDataModel
        {
                Id = 104,
                Name = "Mango Room",
                Capacity = 10,
                Price = 15000
        },
        new HallAPIDataModel
        {
                Id = 103,
                Name = "Ashoka Room",
                Capacity = 30,
                Price = 26000
        },
        new HallAPIDataModel
        {
                Id = 102,
                Name = "Chinar Room",
                Capacity = 50,
                Price = 30000
        }
    };
    if (Id == 0 || bookings.Count() > Id)
    {
        return BadRequest("Operation not performed");
    }
    var i = from l in bookings where l.Id == Id select l;
    i.First().Name = Name;
    return Ok(bookings);

}

[HttpDelete]
[Route("/ID")]
public IActionResult DeleteBookings([FromBody] int Id)
{
    var bookings = new List<HallAPIDataModel>
        {
            new HallAPIDataModel
            {
                Id = 102,
                Name = "Chinar Room",
                Capacity = 50,
                Price = 30000
            },
            new HallAPIDataModel
            {
                Id = 103,
                Name = "Ashoka Room",
                Capacity = 30,
                Price = 26000
            },
            new HallAPIDataModel
            {
                Id = 104,
                Name = "Mango Room",
                Capacity = 10,
                Price = 15000
            }
    };
    var bookingToDelete = bookings.FirstOrDefault(b => b.Id == Id);
    if (bookingToDelete != null)
    {
        bookings.Remove(bookingToDelete);
        return Ok(bookings);
    }
    else
    {
        return NotFound($"Booking with ID {Id} not found.");
    }
}
*/



/*METHOD 2
//USING  JSON FILE TO STORE OUR HALL DATA
[HttpGet]
public ActionResult<List<HallAPIDataModel>> GetBookings()
{
    return JsonFileHelper.ReadFromJsonFile<HallAPIDataModel>();
}

[HttpPost]
public ActionResult<HallAPIDataModel> CreateBookings([FromBody] HallAPIDataModel newBooking)
{
    var bookings = JsonFileHelper.ReadFromJsonFile<HallAPIDataModel>();

    bookings.Add(newBooking);
    JsonFileHelper.WriteToJsonFile(bookings);

    return CreatedAtAction(nameof(GetBookings), new { Id = newBooking.Id }, newBooking);
}

[HttpPut("{id}")]
public ActionResult UpdateBooking(int id, [FromBody] HallAPIDataModel updatedBooking)
{
    var bookings = JsonFileHelper.ReadFromJsonFile<HallAPIDataModel>();
    var booking = bookings.FirstOrDefault(b => b.Id == id);

    if (booking == null)
    {
        return NotFound();
    }

    //Update properties
    booking.Name = updatedBooking.Name;
    booking.Capacity = updatedBooking.Capacity;
    booking.Price = updatedBooking.Price;

    JsonFileHelper.WriteToJsonFile(bookings);

    return NoContent();
}


[HttpPatch("{id}")]
public ActionResult PatchBooking(int id, [FromBody] HallAPIDataModel updatedBooking)
{
    var bookings = JsonFileHelper.ReadFromJsonFile<HallAPIDataModel>();
    var booking = bookings.FirstOrDefault(b => b.Id == id);

    if (booking == null)
    {
        return NotFound();
    }

    // Update only the provided fields
    if (!string.IsNullOrEmpty(updatedBooking.Name))
    {
        booking.Name = updatedBooking.Name;
    }

    if (updatedBooking.Capacity > 0)
    {
        booking.Capacity = updatedBooking.Capacity;
    }

    if (updatedBooking.Price > 0)
    {
        booking.Price = updatedBooking.Price;
    }

    JsonFileHelper.WriteToJsonFile(bookings);

    return NoContent();
}

[HttpDelete("{id}")]
public ActionResult DeleteBooking(int id)
{
    var bookings = JsonFileHelper.ReadFromJsonFile<HallAPIDataModel>();
    var booking = bookings.FirstOrDefault(b => b.Id == id);

    if (booking == null)
    {
        return NotFound();
    }

    bookings.Remove(booking);
    JsonFileHelper.WriteToJsonFile(bookings);

    return NoContent();
}

*/