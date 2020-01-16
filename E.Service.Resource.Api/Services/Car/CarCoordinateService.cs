using E.Service.Resource.Data.Interface.Car;
using E.Service.Resource.Data.Interface.Car.DTO;
using E.Service.Resource.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Api.Services.Car
{
    public class CarCoordinateService : ICarCoordinateService
    {

        private EservicesdbContext db;

        public CarCoordinateService(EservicesdbContext db)
        {
            this.db = db;
        }

        public async Task<IList<CarCoordinateViewDTO>> Get(int carrequestId, int statusID)
        {
            return await db.CarRequestCoordinate.Where(m => m.CarRequestId == carrequestId && m.CarCoordinateStatusId == statusID).Select(m => new CarCoordinateViewDTO()
            {
                CarCoordinateStatusId = m.CarCoordinateStatusId,
                Color = m.CarCoordinateStatus.Color,
                CarRequestId = carrequestId,
                CurrentDateTime = m.CurrentDateTime,
                Id = m.Id,
                carLatLongs = m.CarRequestCoordinateDetail.Select(d => new CarLatLongDTO()
                {
                    CarRequestCoordinateId = d.CarRequestCoordinateId,
                    Id = d.Id,
                    Latitude = d.Latitude,
                    Longitude = d.Longitude,
                    TransactionDateTime = d.TransactionDateTime
                }).ToList()
            }).OrderBy(m => m.CurrentDateTime).ToListAsync();
        }

        public async Task<IList<CarCoordinateViewDTO>> GetAll(int carrequestId)
        {
            return await db.CarRequestCoordinate.Where(m => m.CarRequestId == carrequestId).Select(m => new CarCoordinateViewDTO()
            {
                CarCoordinateStatusId = m.CarCoordinateStatusId,
                Color = m.CarCoordinateStatus.Color,
                CarRequestId = carrequestId,
                CurrentDateTime = m.CurrentDateTime,
                Id = m.Id,
                carLatLongs = m.CarRequestCoordinateDetail.Select(d => new CarLatLongDTO()
                {
                    CarRequestCoordinateId = d.CarRequestCoordinateId,
                    Id = d.Id,
                    Latitude = d.Latitude,
                    Longitude = d.Longitude,
                    TransactionDateTime = d.TransactionDateTime
                }).ToList()
            }).OrderBy(m => m.CurrentDateTime).ToListAsync();
        }

        public async Task<string> GetLastStatus(int carRequestId)
        {
            var data = await db.CarRequestCoordinate
                .Where(m => m.CarRequestId == carRequestId)
                .OrderByDescending(m => m.Id)
                .Include(m => m.CarCoordinateStatus)
                .FirstOrDefaultAsync();

            return data == null ? "" : data.CarCoordinateStatus.Name;


        }
    }
}
