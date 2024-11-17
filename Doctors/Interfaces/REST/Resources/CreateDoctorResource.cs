﻿using System.ComponentModel.DataAnnotations;

namespace HealMeAppBackend.API.Doctors.Interfaces.REST.Resources
{
    /// <summary>
    /// Represents the data provided by the client to create a doctor.
    /// </summary>
    /// <param name="Name">The doctor's name</param>
    /// <param name="Description">The doctor's description</param>
    /// <param name="Rating">The doctor's rating (0 to 5 stars)</param>
    public record CreateDoctorResource(
        [Required] string Name,
        [Required] string Description,
        [Required, Range(0, 5)] int Rating
    );
}
