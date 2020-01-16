using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E.Service.Resource.Data.Interface.Car.DTO
{
    public class CarCoordinateViewDTO
    {
        public long Id { get; set; }
        public int CarRequestId { get; set; }
        public int CarCoordinateStatusId { get; set; }
        public DateTime CurrentDateTime { get; set; }
        public string Color { get; set; }
        public List<CarLatLongDTO> carLatLongs { get; set; }
    }


    public class CarLatLongDTO
    {
        public long Id { get; set; }
        public long CarRequestCoordinateId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime TransactionDateTime { get; set; }
    }

    public class CarCoordinateInsertDTO
    {
        public long Id { get; set; }
        public int CarRequestId { get; set; }
        public int CarCoordinateStatusId { get; set; }
        public DateTime CurrentDateTime { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

    }
}
