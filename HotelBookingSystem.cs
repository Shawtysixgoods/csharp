using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelBookingSystem
{
    // Interface for generic room operations
    public interface IRoom
    {
        int RoomNumber { get; }
        decimal PricePerNight { get; }
        bool IsAvailable { get; }
        void Book();
        void Release();
        string GetDetails();
    }

    // Abstract base class demonstrating Inheritance and Abstraction
    public abstract class Room : IRoom
    {
        public int RoomNumber { get; private set; }
        public decimal PricePerNight { get; protected set; }
        public bool IsAvailable { get; private set; }

        public Room(int roomNumber, decimal price)
        {
            RoomNumber = roomNumber;
            PricePerNight = price;
            IsAvailable = true;
        }

        public void Book()
        {
            if (!IsAvailable)
                throw new InvalidOperationException($"Room {RoomNumber} is already booked.");
            IsAvailable = false;
        }

        public void Release()
        {
            IsAvailable = true;
        }

        // Virtual method demonstrating Polymorphism
        public virtual string GetDetails()
        {
            return $"Room: {RoomNumber}, Price: ${PricePerNight}/night, Available: {IsAvailable}";
        }
    }

    // Derived class 1
    public class StandardRoom : Room
    {
        public bool HasTwinBeds { get; set; }

        public StandardRoom(int roomNumber, decimal price, bool hasTwinBeds)
            : base(roomNumber, price)
        {
            HasTwinBeds = hasTwinBeds;
        }

        public override string GetDetails()
        {
            string bedType = HasTwinBeds ? "Twin Beds" : "Double Bed";
            return base.GetDetails() + $", Type: Standard, Bed: {bedType}";
        }
    }

    // Derived class 2
    public class SuiteRoom : Room
    {
        public int NumberOfRooms { get; set; }
        public bool HasJacuzzi { get; set; }

        public SuiteRoom(int roomNumber, decimal price, int numberOfRooms, bool hasJacuzzi)
            : base(roomNumber, price)
        {
            NumberOfRooms = numberOfRooms;
            HasJacuzzi = hasJacuzzi;
        }

        public override string GetDetails()
        {
            string jacuzziInfo = HasJacuzzi ? "Yes" : "No";
            return base.GetDetails() + $", Type: Suite, Rooms: {NumberOfRooms}, Jacuzzi: {jacuzziInfo}";
        }
    }

    // Represents a customer
    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public Customer(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public string GetFullName() => $"{FirstName} {LastName}";
    }

    // Represents a reservation
    public class Reservation
    {
        private static int _idCounter = 1000;
        public int ReservationId { get; private set; }
        public Customer Guest { get; private set; }
        public IRoom ReservedRoom { get; private set; }
        public int Nights { get; private set; }
        public decimal TotalCost => ReservedRoom.PricePerNight * Nights;

        public Reservation(Customer guest, IRoom room, int nights)
        {
            ReservationId = ++_idCounter;
            Guest = guest;
            ReservedRoom = room;
            Nights = nights;
        }

        public void CompleteBooking()
        {
            ReservedRoom.Book();
            Console.WriteLine($"Reservation #{ReservationId} confirmed for {Guest.GetFullName()}.");
            Console.WriteLine($"Room: {ReservedRoom.RoomNumber} for {Nights} nights.");
            Console.WriteLine($"Total Cost: ${TotalCost}");
        }
    }

    // Main manager class
    public class HotelManager
    {
        private List<IRoom> _rooms;
        private List<Reservation> _reservations;

        public HotelManager()
        {
            _rooms = new List<IRoom>();
            _reservations = new List<Reservation>();
        }

        public void AddRoom(IRoom room)
        {
            _rooms.Add(room);
        }

        public void ShowAvailableRooms()
        {
            Console.WriteLine("\n--- Available Rooms ---");
            var availableRooms = _rooms.Where(r => r.IsAvailable).ToList();

            if (!availableRooms.Any())
            {
                Console.WriteLine("No rooms available.");
                return;
            }

            foreach (var room in availableRooms)
            {
                Console.WriteLine(room.GetDetails());
            }
        }

        public void CreateReservation(Customer customer, int roomNumber, int nights)
        {
            var room = _rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
            if (room == null)
            {
                Console.WriteLine($"Error: Room {roomNumber} does not exist.");
                return;
            }

            if (!room.IsAvailable)
            {
                Console.WriteLine($"Error: Room {roomNumber} is currently not available.");
                return;
            }

            try
            {
                var reservation = new Reservation(customer, room, nights);
                reservation.CompleteBooking();
                _reservations.Add(reservation);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create reservation: {ex.Message}");
            }
        }

        public void ShowReservations()
        {
            Console.WriteLine("\n--- Current Reservations ---");
            if (!_reservations.Any())
            {
                Console.WriteLine("No active reservations.");
                return;
            }

            foreach (var res in _reservations)
            {
                Console.WriteLine($"[{res.ReservationId}] {res.Guest.GetFullName()} - Room {res.ReservedRoom.RoomNumber} - ${res.TotalCost} ({res.Nights} nights)");
            }
        }
    }

    // Application entry point
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the OOP Hotel Booking System!");

            HotelManager manager = new HotelManager();

            // Seed some data
            manager.AddRoom(new StandardRoom(101, 50m, false));
            manager.AddRoom(new StandardRoom(102, 55m, true));
            manager.AddRoom(new StandardRoom(103, 50m, false));
            manager.AddRoom(new SuiteRoom(201, 150m, 2, false));
            manager.AddRoom(new SuiteRoom(202, 200m, 3, true));

            manager.ShowAvailableRooms();

            Console.WriteLine("\nProcessing booking for John Doe...");
            Customer customer1 = new Customer("John", "Doe", "john.doe@example.com");
            manager.CreateReservation(customer1, 102, 3);

            Console.WriteLine("\nProcessing booking for Jane Smith...");
            Customer customer2 = new Customer("Jane", "Smith", "jane.smith@example.com");
            manager.CreateReservation(customer2, 202, 2);

            // Attempt to double-book
            Console.WriteLine("\nAttempting to book an already booked room...");
            Customer customer3 = new Customer("Bob", "Johnson", "bob@example.com");
            manager.CreateReservation(customer3, 102, 1);

            manager.ShowAvailableRooms();
            manager.ShowReservations();

            Console.WriteLine("\nThank you for using the system.");
        }
    }
}
