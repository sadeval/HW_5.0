using System;

namespace EventManagementSystem
{
    class Program
    {
        static void Main()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                var event1 = new Event { Name = "Tech Conference", Date = DateTime.Now };
                var event2 = new Event { Name = "Business Summit", Date = DateTime.Now.AddMonths(1) };
                var guest1 = new Guest { Name = "Mia Rosso" };
                var guest2 = new Guest { Name = "Natalie Smith" };

                db.Events.AddRange(event1, event2);
                db.Guests.AddRange(guest1, guest2);
                db.SaveChanges();

                var manager = new EventService(db);

                manager.AddGuestToEvent(event1.EventId, guest1.GuestId, "Спикер");

                var guestsAtEvent = manager.GetGuestsByEvent(event1.EventId);
                foreach (var guest in guestsAtEvent)
                {
                    Console.WriteLine($"Гость: {guest.Name}");
                }


                manager.UpdateGuestRole(event1.EventId, guest1.GuestId, "Участник");


                var eventsForGuest = manager.GetEventsByGuest(guest1.GuestId);
                foreach (var ev in eventsForGuest)
                {
                    Console.WriteLine($"Событие для {guest1.Name}: {ev.Name}");
                }


                manager.RemoveGuestFromEvent(event1.EventId, guest1.GuestId);


                var speakerEvents = manager.GetEventsWhereGuestIsSpeaker(guest2.GuestId);
                foreach (var ev in speakerEvents)
                {
                    Console.WriteLine($"Событие, где {guest2.Name} был спикером: {ev.Name}");
                }
            }
        }
    }
}
