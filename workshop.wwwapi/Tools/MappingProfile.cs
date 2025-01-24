using AutoMapper;
using workshop.wwwapi.DTO;
using workshop.wwwapi.Models;

namespace workshop.wwwapi.Tools
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Doctor, DoctorView>();
            CreateMap<Doctor, DoctorInternal>();

            CreateMap<Appointment, AppointmentView>();
            CreateMap<Appointment, AppointmentInternal>();
            CreateMap<Appointment, AppointmentPatient>();
            CreateMap<Appointment, AppointmentPatientDoctor>();

            CreateMap<Patient, PatientView>();
            CreateMap<Patient, PatientInternal>();
        }
    }
}
