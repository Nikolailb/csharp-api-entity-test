namespace workshop.wwwapi.DTO
{
    public record AppointmentView();
    public record AppointmentInternal(DateTime Booking);
    public record AppointmentPatient(DateTime Booking, PatientInternal Patient);
    public record AppointmentPatientDoctor(DateTime Booking, DoctorInternal Doctor);

}
