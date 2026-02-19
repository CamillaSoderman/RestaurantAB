using RestaurantAB.DTOs;
using RestaurantAB.DTOs.ReservationDTOs;
using RestaurantAB.DTOs.TableDTOs;
using RestaurantAB.Models;
using RestaurantAB.Repository.IRepository;
using RestaurantAB.Services.IServices;

namespace RestaurantAB.Services.Implementation
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationRepository _resRepo;

        public ReservationService(IReservationRepository resRepo)
        {
            _resRepo = resRepo;
        }

        // -------------------- CREATE CUSTOMER --------------------
        public async Task<int> CreateCustomerAsync(CustomerDTO custDTO)
        {
            var customer = new Customer
            {
                CustomerName = custDTO.CustomerName,
                CustomerPhone = custDTO.CustomerPhone,
                CustomerEmail = custDTO.CustomerEmail
            };

            return await _resRepo.CreateCustomerAsync(customer);
        }

        // -------------------- CREATE RESERVATION --------------------
        public async Task<int> CreateReservationAsync(ReservationRequestDTO request)
        {
            // Check table availability
            var isOccupied = await _resRepo.IsTableOccupiedAsync(request.TableId, request.StartTime);
            if (isOccupied)
                throw new InvalidOperationException("The table is already booked for the selected time.");

            // Validate table capacity
            var table = await _resRepo.GetTableByIdAsync(request.TableId);
            if (table == null || table.Capacity < request.Guests)
                throw new InvalidOperationException("The table does not exist or cannot accommodate the number of guests.");

            // Find or create customer
            var existingCustomer = await _resRepo.GetCustomerByEmailAsync(request.CustomerEmail);
            int customerId;

            if (existingCustomer != null)
            {
                customerId = existingCustomer.CustomerId;
            }
            else
            {
                var newCustomer = new Customer
                {
                    CustomerName = request.CustomerName,
                    CustomerEmail = request.CustomerEmail
                };

                customerId = await _resRepo.CreateCustomerAsync(newCustomer);
            }

            // Create reservation
            var reservation = new Reservation
            {
                TableId = request.TableId,
                CustomerId = customerId,
                StartTime = request.StartTime,
                EndTime = request.StartTime.AddHours(2),
                NumberOfGuests = request.Guests
            };

            return await _resRepo.CreateReservationAsync(reservation);
        }

        // -------------------- DELETE RESERVATION --------------------
        public async Task<bool> DeleteReservationAsync(int id)
        {
            return await _resRepo.DeleteReservationAsync(id);
        }

        // -------------------- CUSTOMER RESERVATIONS --------------------
        public async Task<List<ReservationDTO>> GetAllReservationsForCustomerAsync(string email)
        {
            var reservations = await _resRepo.GetReservationsByCustomerEmailAsync(email);

            return reservations.Select(res => new ReservationDTO
            {
                ResId = res.ResId,
                TableId = res.TableId,
                StartTime = res.StartTime,
                EndTime = res.EndTime,
                NumberOfGuests = res.NumberOfGuests,
                CustomerName = res.Customer.CustomerName
            }).ToList();
        }

        // -------------------- ADMIN: ALL RESERVATIONS --------------------
        public async Task<List<ReservationAdminDTO>> GetAllReservationsAsync()
        {
            var reservations = await _resRepo.GetAllReservationsAsync();

            return reservations.Select(r => new ReservationAdminDTO
            {
                ResId = r.ResId,
                TableId = r.TableId,
                CustomerId = r.CustomerId,
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                NumberOfGuests = r.NumberOfGuests,
                CustomerName = r.Customer.CustomerName,
                CustomerEmail = r.Customer.CustomerEmail
            }).ToList();
        }

        // -------------------- ADMIN: RESERVATION BY ID --------------------
        public async Task<ReservationAdminDTO?> GetReservationByIdAsync(int resId)
        {
            var reservation = await _resRepo.GetReservationByIdAsync(resId);
            if (reservation == null)
                return null;

            return new ReservationAdminDTO
            {
                ResId = reservation.ResId,
                TableId = reservation.TableId,
                CustomerId = reservation.CustomerId,
                StartTime = reservation.StartTime,
                EndTime = reservation.EndTime,
                NumberOfGuests = reservation.NumberOfGuests,
                CustomerName = reservation.Customer.CustomerName,
                CustomerEmail = reservation.Customer.CustomerEmail
            };
        }


        //public async Task<bool> UpdateReservationAsync(int id, ReservationDTO resDTO)
        //{
        //    var existingRes = await _resRepo.GetReservationByIdAsync(resDTO.ResId);

        //    if (existingRes == null)
        //        return false;

        //    existingRes.TableId = resDTO.TableId;
        //    existingRes.StartTime = resDTO.StartTime;
        //    existingRes.EndTime = resDTO.StartTime.AddHours(2);
        //    existingRes.NumberOfGuests = resDTO.NumberOfGuests;

        //    await _resRepo.UpdateReservationAsync(existingRes);

        //    return true;
        //}

        // -------------------- ADMIN: UPDATE RESERVATION --------------------
        public async Task<bool> UpdateReservationAsync(int id, ReservationUpdateDTO dto)
        {
            var existingRes = await _resRepo.GetReservationByIdAsync(dto.ResId);

            if (existingRes == null)
                return false;

            existingRes.TableId = dto.TableId;
            existingRes.StartTime = dto.StartTime;
            existingRes.EndTime = dto.StartTime.AddHours(2);
            existingRes.NumberOfGuests = dto.NumberOfGuests;

            await _resRepo.UpdateReservationAsync(existingRes);
            return true;
        }

        // -------------------- TABLE LOOKUPS --------------------
        public async Task<TableDTO?> GetBestAvailableTableAsync(DateTime startTime, int guests)
        {
            var table = await _resRepo.GetBestAvailableTableAsync(startTime, guests);
            if (table == null)
                return null;

            return new TableDTO
            {
                TableId = table.TableId,
                Capacity = table.Capacity
            };
        }

        public async Task<List<TableDTO>> GetAllAvailableTablesAsync(DateTime startTime, int guests)
        {
            var tables = await _resRepo.GetAllAvailableTablesAsync(startTime, guests);

            return tables.Select(t => new TableDTO
            {
                TableId = t.TableId,
                Capacity = t.Capacity
            }).ToList();
        }

        public async Task<TableDTO?> GetTableByIdAsync(int tableId)
        {
            var table = await _resRepo.GetTableByIdAsync(tableId);
            if (table == null)
                return null;

            return new TableDTO
            {
                TableId = table.TableId,
                Capacity = table.Capacity
            };
        }
    }
}
//using RestaurantAB.DTOs;
//using RestaurantAB.DTOs.ReservationDTOs;
//using RestaurantAB.DTOs.TableDTOs;
//using RestaurantAB.Models;
//using RestaurantAB.Repository.IRepository;
//using RestaurantAB.Services.IServices;

//namespace RestaurantAB.Services.Implementation
//{
//    public class ReservationService : IReservationService
//    {
//        private readonly IReservationRepository _resRepo;

//        public ReservationService(IReservationRepository resRepo)
//        {
//            _resRepo = resRepo;
//        }
//        public async Task<int> CreateCustomerAsync(CustomerDTO custDTO)
//        {
//            var customer = new Customer
//            {
//                CustomerName = custDTO.CustomerName,
//                CustomerPhone = custDTO.CustomerPhone,
//                CustomerEmail = custDTO.CustomerEmail
//            };

//            var newCustId = await _resRepo.CreateCustomerAsync(customer);
//            return newCustId;
//        }
//        public async Task<int> CreateReservationAsync(ReservationRequestDTO request)
//        {
//            var startTime = request.StartTime;
//            var endTime = startTime.AddHours(2); // Setting reservation duration to 2 hours

//            var isOccupied = await _resRepo.IsTableOccupiedAsync(request.TableId, request.StartTime);
//            if (isOccupied)
//            {
//                throw new InvalidOperationException("The table is already booked for the selected time.");
//            }

//            var table = await _resRepo.GetTableByIdAsync(request.TableId);
//            if (table == null || table.Capacity < request.Guests)
//            {
//                throw new InvalidOperationException("The table does not exist or cannot accommodate the number of guests.");
//            }



//            // Check if customer already exists by email
//            var existingCustomer = await _resRepo.GetCustomerByEmailAsync(request.CustomerEmail);

//            int customerId;
//            if (existingCustomer != null)
//            {
//                customerId = existingCustomer.CustomerId;
//            }
//            else
//            {
//                // New customer
//                var newCustomer = new Customer
//                {
//                    CustomerName = request.CustomerName,
//                    //CustomerPhone = request.CustomerPhone,
//                    CustomerEmail = request.CustomerEmail
//                };
//                customerId = await _resRepo.CreateCustomerAsync(newCustomer);
//            }



//            // If available, create the reservation
//            var reservation = new Reservation
//            {
//                TableId = request.TableId,
//                CustomerId = customerId, // From system, not input from customer
//                StartTime = request.StartTime,
//                EndTime = request.StartTime.AddHours(2),
//                NumberOfGuests = request.Guests
//            };

//            var newResId = await _resRepo.CreateReservationAsync(reservation);
//            return newResId;


//        }

//        public async Task<bool> DeleteReservationAsync(int id)
//        {
//            var deletedRes = await _resRepo.DeleteReservationAsync(id);
//            if (!deletedRes)
//            {
//                return false;
//            }
//            return true;
//        }

//        public async Task<List<ReservationDTO>> GetAllReservationsForCustomerAsync(string email)
//        {
//            var reservations = await _resRepo.GetReservationsByCustomerEmailAsync(email);

//            return reservations.Select(res => new ReservationDTO
//            {
//                ResId = res.Id,
//                TableId = res.TableId,
//                StartTime = res.StartTime,
//                NumberOfGuests = res.NumberOfGuests,
//                CustomerName = res.Customer.CustomerName ?? "Unknown"
//            }).ToList();
//        }

//        public async Task<List<ReservationAdminDTO>> GetAllReservationsForAdminAsync()
//        {
//            var reservations = await _resRepo.GetAllReservationsAsync();
//            return reservations.Select(res => new ReservationAdminDTO
//            {
//                ResId = res.Id,
//                TableId = res.TableId,
//                CustomerId = res.CustomerId,
//                StartTime = res.StartTime,
//                EndTime = res.EndTime,
//                NumberOfGuests = res.NumberOfGuests,
//                CustomerName = res.Customer.CustomerName,
//                CustomerEmail = res.Customer.CustomerEmail,
//            }).ToList();
//        }


//        //public async Task<ReservationAdminDTO> GetReservationByIdAsync(int id)
//        //{
//        //    var res = await _resRepo.GetReservationByIdAsync(id);
//        //    if (res == null)
//        //    {
//        //      throw new InvalidOperationException("No reservation found."); ;
//        //    }

//        //    var resDTO = new ReservationAdminDTO
//        //    {
//        //        ResId = res.ResId,
//        //        TableId = res.TableId,
//        //        CustomerId = res.CustomerId,
//        //        StartTime = res.StartTime,
//        //        NumberOfGuests = res.NumberOfGuests
//        //    };

//        //    return resDTO;
//        //}

//        public async Task<bool> UpdateReservationAsync(int id, ReservationDTO resDTO)
//        {
//            var existingRes = await _resRepo.GetReservationByIdAsync(resDTO.ResId);

//            if (existingRes == null)
//            {
//                return false;
//            }

//            // Show reservation details before update
//            existingRes.TableId = resDTO.TableId;
//            existingRes.StartTime = resDTO.StartTime;
//            existingRes.EndTime = resDTO.StartTime.AddHours(2); // booking will always be 2 hours
//            existingRes.NumberOfGuests = resDTO.NumberOfGuests;

//            Console.WriteLine($"Existing Reservation: " +
//                $"ResId={existingRes.Id}, " +
//                $"TableId={existingRes.TableId}, " +
//                $"StartTime={existingRes.StartTime}, " +
//                $"EndTime={existingRes.EndTime}, " +
//                $"NumberOfGuests={existingRes.NumberOfGuests}");


//            await _resRepo.UpdateReservationAsync(existingRes);

//            return true;
//        }
//        public async Task<TableDTO?> GetBestAvailableTableAsync(DateTime startTime, int guests)
//        {
//            var table = await _resRepo.GetBestAvailableTableAsync(startTime, guests);

//            if (table == null) return null;

//            return new TableDTO
//            {
//                TableId = table.TableId,
//                Capacity = table.Capacity
//            };
//        }

//        public async Task<List<TableDTO>> GetAllAvailableTablesAsync(DateTime startTime, int guests)
//        {
//            var tables = await _resRepo.GetAllAvailableTablesAsync(startTime, guests);
//            return tables.Select(t => new TableDTO
//            {
//                TableId = t.TableId,
//                Capacity = t.Capacity
//            }).ToList();
//        }




//        public async Task<List<ReservationAdminDTO>> GetAllReservationsAsync()
//        {
//            var reservations = await _resRepo.GetAllReservationsAsync();
//            return reservations.Select(r => new ReservationAdminDTO
//            {
//                ResId = r.Id,
//                TableId = r.TableId,
//                CustomerId = r.CustomerId,
//                StartTime = r.StartTime,
//                EndTime = r.EndTime,
//                NumberOfGuests = r.NumberOfGuests,
//                CustomerName = r.Customer.CustomerName,
//                CustomerEmail = r.Customer.CustomerEmail,
//            }).ToList();
//        }


//        public async Task<ReservationAdminDTO?> GetReservationByIdAsync(int resId)
//        {
//            var reservation = await _resRepo.GetReservationByIdAsync(resId);
//            if (reservation == null)
//                return null;

//            return new ReservationAdminDTO
//            {
//                ResId = reservation.Id,
//                TableId = reservation.TableId,
//                CustomerId = reservation.CustomerId,
//                StartTime = reservation.StartTime,
//                EndTime = reservation.EndTime,
//                NumberOfGuests = reservation.NumberOfGuests,
//                CustomerName = reservation.Customer.CustomerName,
//                CustomerEmail = reservation.Customer.CustomerEmail,
//            };
//        }


//        public async Task<TableDTO?> GetTableByIdAsync(int tableId)
//        {
//            var table = await _resRepo.GetTableByIdAsync(tableId);
//            if (table == null) return null;

//            return new TableDTO
//            {
//                TableId = table.TableId,
//                Capacity = table.Capacity
//            };
//        }
//    }
//}
