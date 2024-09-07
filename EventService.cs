using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EventManagementSystem
{
    public class EventService
    {
        private readonly ApplicationContext _context;

        public EventService(ApplicationContext context)
        {
            _context = context;
        }

        public void AddGuestToEvent(int eventId, int guestId, string role)
        {
            var eventGuest = new EventGuest
            {
                EventId = eventId,
                GuestId = guestId,
                Role = role
            };

            _context.EventGuests.Add(eventGuest);
            _context.SaveChanges();
        }

        public List<Guest> GetGuestsByEvent(int eventId)
        {
            return _context.EventGuests
                .Where(eg => eg.EventId == eventId)
                .Include(eg => eg.Guest)
                .Select(eg => eg.Guest)
                .ToList();
        }

        public void UpdateGuestRole(int eventId, int guestId, string newRole)
        {
            var eventGuest = _context.EventGuests
                .FirstOrDefault(eg => eg.EventId == eventId && eg.GuestId == guestId);

            if (eventGuest != null)
            {
                eventGuest.Role = newRole;
                _context.SaveChanges();
            }
        }

        public List<Event> GetEventsByGuest(int guestId)
        {
            return _context.EventGuests
                .Where(eg => eg.GuestId == guestId)
                .Include(eg => eg.Event)
                .Select(eg => eg.Event)
                .ToList();
        }

        public void RemoveGuestFromEvent(int eventId, int guestId)
        {
            var eventGuest = _context.EventGuests
                .FirstOrDefault(eg => eg.EventId == eventId && eg.GuestId == guestId);

            if (eventGuest != null)
            {
                _context.EventGuests.Remove(eventGuest);
                _context.SaveChanges();
            }
        }

        public List<Event> GetEventsWhereGuestIsSpeaker(int guestId)
        {
            return _context.EventGuests
                .Where(eg => eg.GuestId == guestId && eg.Role == "Speaker")
                .Include(eg => eg.Event)
                .Select(eg => eg.Event)
                .ToList();
        }
    }
}
