namespace workshop.wwwapi.DTO
{
    public record AppointmentPost(int DoctorId, int PatientId, int DaysTilBooking);
    public record AppointmentView(DateTime Booking, DoctorInternal Doctor, PatientInternal Patient);
    public record AppointmentInternal(DateTime Booking);
    public record AppointmentPatient(DateTime Booking, PatientInternal Patient);
    public record AppointmentDoctor(DateTime Booking, DoctorInternal Doctor);

}
