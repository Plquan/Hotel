using AutoMapper;
using Hotel.Data;
using Hotel.Data.Dtos;
using Hotel.Data.Models;
using Hotel.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Services
{
    public interface IRoomTypeRepository
    {      
        Task Add(RoomTypeDTO roomType);
        Task Delete(int id);
        Task Update(RoomTypeDTO room);
        Task<List<RoomType>> GetAll();
        Task<RoomTypeVM> GetById(int id);
    }


    public class RoomTypeRepository : IRoomTypeRepository
    {
        private readonly HotelContext _context;
        private readonly IMapper _mapper;
        private readonly IFileServices _fileServices;
        public RoomTypeRepository(HotelContext context, IMapper mapper,IFileServices fileServices)
        {
            _context = context;
            _mapper = mapper;
            _fileServices = fileServices;
            
        }

        public int GetRoomId(RoomTypeDTO roomType)
        {
            var newroomtype = new RoomType() {
                Name = roomType.Name,
                Content = roomType.Content,
                Capacity = roomType.Capacity,
                Price = roomType.Price,
                Quantity = 0,
                View = roomType.View,
                BedType = roomType.BedType,
                Thumb = roomType.Thumb != null ? _fileServices.Upload(roomType.Thumb) : " ",
                Status = roomType.Status
            };

          _context.RoomTypes.Add(newroomtype);
			 _context.SaveChanges();
            return newroomtype.Id;
		}

        public async Task Add(RoomTypeDTO roomType)
        {
            var Id = GetRoomId(roomType); 

            if (roomType.RoomFacilitys != null) {
                foreach (var facility in roomType.RoomFacilitys)
                {
                    var newTag = new Data.Models.RoomFacility()
                    {
                        RoomTypeId = Id,
                        Name = facility
                    };
                    _context.RoomFacilitys.Add(newTag);
                }
            }
            if (roomType.RoomImages != null)
            {
                foreach(var image in roomType.RoomImages)
                {
                    var img = _fileServices.Upload(image);
                    var newImage = new RoomImage()
                    {
                        RoomTypeId = Id,
                        Url = img                    
                    };
                    _context.RoomImages.Add(newImage);
                }
            }
            if(roomType.RoomServices != null)
            {
                foreach(var service in roomType.RoomServices)
                {
                    var newService = new RoomService() 
                    {
                        RoomTypeId = Id,
                        Name = service
                    };
                    _context.RoomServices.Add(newService);
                }
            }
            await _context.SaveChangesAsync();
        }


        public async Task Delete(int id)
        {
            var roomtype = _context.RoomTypes.FirstOrDefault(r => r.Id == id);
            if (roomtype != null)
            {
                _context.Remove(roomtype);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<RoomType>> GetAll()
        {
            return await _context.RoomTypes.ToListAsync();
        }

		public async Task<RoomTypeVM> GetById(int id)
		{
			var room = await _context.RoomTypes.FirstOrDefaultAsync(x => x.Id == id);
            if (room == null)
            {
                throw new Exception("Không tìm thấy phòng!");
            }
            var tag = await _context.RoomFacilitys.Where(x => x.RoomTypeId == id).ToListAsync();
			var image = await _context.RoomImages.Where(x => x.RoomTypeId == id).ToListAsync();
			var service = await _context.RoomServices.Where(x => x.RoomTypeId == id).ToListAsync();
                     
			var roomtype = new RoomTypeVM()
			{
				Name = room.Name,
                Capacity = room.Capacity,
                View = room.View,
                BedType = room.BedType,
                Price = room.Price,
                Status = room.Status,
                Content = room.Content,
                Thumb = room.Thumb,
                RoomServices = _mapper.Map<List<RoomServiceDTO>>(service),
                RoomImages = _mapper.Map<List<RoomImageDTO>>(image),
                RoomFacilitys = _mapper.Map<List<RoomFacilityDTO>>(tag)
			};
            return roomtype;
		}

        public async Task Update(RoomTypeDTO roomTypeDTO)
        {
            var roomtype = _context.RoomTypes.FirstOrDefault(x => x.Id == roomTypeDTO.Id);

            if (roomtype != null)
            {
                var newthumb =  _fileServices.Upload(roomTypeDTO.Thumb);
                 _fileServices.Delete(roomtype.Thumb);
                roomtype.Name = roomTypeDTO.Name;
                roomtype.Content = roomTypeDTO.Content;
                roomtype.Capacity = roomTypeDTO.Capacity;
                roomtype.View = roomTypeDTO.View;
                roomtype.BedType = roomTypeDTO.BedType;
                roomtype.Price = roomTypeDTO.Price;
                roomtype.Status = roomTypeDTO.Status;
                roomtype.Thumb = newthumb;

                if (roomTypeDTO.RoomImages != null)
                {
                    var roomImg = _context.RoomImages.Where(x => x.Id == roomtype.Id).ToList();
                    _context.RoomImages.RemoveRange(roomImg);

                    foreach (var img in roomTypeDTO.RoomImages)
                    {
                        var imgCloudinary =  _fileServices.Upload(img);
                           _fileServices.Delete(img);
                        var newImg = new RoomImage()
                        {
                            RoomTypeId = roomtype.Id,
                            Url = imgCloudinary
                        };
                        _context.RoomImages.Add(newImg);
                    }
                }

                if (roomTypeDTO.RoomFacilitys != null)
                {
                    var roomTag = _context.RoomFacilitys.Where(x => x.RoomTypeId == roomtype.Id).ToList();
                       _context.RemoveRange(roomTag);
                       foreach (var facility in roomTypeDTO.RoomFacilitys)
                        {
                        var newtag = new RoomFacility()
                        {
                            RoomTypeId = roomtype.Id,
                            Name = facility
                        };
                        _context.RoomFacilitys.Add(newtag);                                                     
                        }
                }
                if (roomTypeDTO.RoomServices != null)
                {
                   var roomService = _context.RoomServices.Where(x => x.RoomTypeId == roomtype.Id).ToList();
                    _context.RemoveRange(roomService);
                    foreach(var service in roomTypeDTO.RoomServices)
                    {
                        var newRoomService = new RoomService() { 
                            RoomTypeId = roomtype.Id,
                            Name = service
                        };
                        _context.RoomServices.Add(newRoomService);

                    }
                }
                    await _context.SaveChangesAsync();

                }
            }

        }



    }

