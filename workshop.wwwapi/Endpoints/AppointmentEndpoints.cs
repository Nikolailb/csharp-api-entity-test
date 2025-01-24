using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using workshop.wwwapi.DTO;
using workshop.wwwapi.Exceptions;
using workshop.wwwapi.Models;
using workshop.wwwapi.Repository;

namespace workshop.wwwapi.Endpoints
{
    public static class AppointmentEndpoints
    {
        public static string Path { get; private set; } = "appointments";

        public static void ConfigureAppointmentEndpoints(this WebApplication app)
        {
            var group = app.MapGroup(Path);

            group.MapPost("/", CreateDoctor);
            group.MapGet("/", GetDoctors);
            group.MapGet("/{id}", GetDcotor);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> CreateDoctor(IRepository<Doctor, int> repository, DoctorPost entity)
        {
            try
            {
                Doctor doctor = await repository.Add(new Doctor
                {
                    FirstName = entity.FirstName,
                    LastName = entity.LastName,
                });
                return TypedResults.Created($"/{Path}/{doctor.Id}", new DoctorView(
                    doctor.Id,
                    doctor.FullName,
                    doctor.Appointments.Select(a =>
                        new AppointmentPatient(
                            a.Booking,
                            new PatientInternal(
                                a.Patient.Id,
                                a.Patient.FullName
                            )
                        )
                    )
                ));
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> GetDoctors(IRepository<Doctor, int> repository)
        {
            try
            {
                IEnumerable<Doctor> doctors = await repository.GetAllWithIncludes(
                    q => q.Include(x => x.Appointments).ThenInclude(x => x.Patient)
                );
                return TypedResults.Ok(doctors.Select(d => new DoctorView(
                        d.Id,
                        d.FullName,
                        d.Appointments.Select(appointment => new AppointmentPatient(
                            appointment.Booking,
                            new PatientInternal(
                                appointment.Patient.Id,
                                appointment.Patient.FullName
                            )
                        ))
                    )
                ));
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetDcotor(IRepository<Doctor, int> repository, int id)
        {
            try
            {
                Doctor doctor = await repository.Get(id);
                return TypedResults.Ok(new DoctorView(
                    doctor.Id,
                    doctor.FullName,
                    doctor.Appointments.Select(appointment => new AppointmentPatient(
                        appointment.Booking,
                        new PatientInternal(
                            appointment.Patient.Id,
                            appointment.Patient.FullName
                        )
                    ))
                ));
            }
            catch (IdNotFoundException ex)
            {
                return TypedResults.NotFound(new { ex.Message });
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }
    }
}
