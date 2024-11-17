﻿using System.Net.Mime;
using HealMeAppBackend.API.Hospitals.Application.Internal;
using HealMeAppBackend.API.Hospitals.Domain.Model.Queries;
using HealMeAppBackend.API.Hospitals.Domain.Services;
using HealMeAppBackend.API.Hospitals.Interfaces.REST.Resources;
using HealMeAppBackend.API.Hospitals.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HealMeAppBackend.API.Hospitals.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Hospitals")]
public class HospitalsController(
    IHospitalCommandService hospitalCommandService,
    IHospitalQueryService hospitalQueryService) : ControllerBase
{
    [HttpPost]
    [Authorize]
    [SwaggerOperation(
        Summary = "Create a hospital",
        Description = "Create a hospital with a given name, description, location, and rating",
        OperationId = "CreateHospital"
        )]
    [SwaggerResponse(201, "The hospital was created", typeof(HospitalResource))]
    public async Task<ActionResult> CreateHospital([FromBody] CreateHospitalResource resource)
    {
        var createHospitalCommand = CreateHospitalCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await hospitalCommandService.Handle(createHospitalCommand);

        if (result is null) return BadRequest();

        return CreatedAtAction(nameof(GetHospitalById), new { id = result.Id }, HospitalResourceFromEntityAssembler.ToResourceFromEntity(result));
    }

    [HttpGet]
    [Authorize]
    [SwaggerOperation(
        Summary = "Gets a hospital according to parameters",
        Description = "Gets a hospital for a given name",
        OperationId = "GetHospitalByName"
        )]
    [SwaggerResponse(200, "Result(s) was/were found", typeof(HospitalResource))]
    public async Task<ActionResult> GetHospitalByName([FromQuery] string name)
    {
        var getHospitalByNameQuery = new GetHospitalByNameQuery(name);
        var result = await hospitalQueryService.Handle(getHospitalByNameQuery);
        if (result is null) return BadRequest();
        var resource = HospitalResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    [HttpGet("{id}")]
    [Authorize]
    [SwaggerOperation(
        Summary = "Gets a hospital by id",
        Description = "Gets a hospital for a given hospital identifier",
        OperationId = "GetHospitalById"
        )]
    [SwaggerResponse(200, "The hospital was found", typeof(HospitalResource))]
    public async Task<ActionResult> GetHospitalById(int id)
    {
        var getHospitalByIdQuery = new GetHospitalByIdQuery(id);
        var result = await hospitalQueryService.Handle(getHospitalByIdQuery);
        if (result is null) return BadRequest();
        var resource = HospitalResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    [HttpGet("rating/{rating}")]
    [Authorize]
    [SwaggerOperation(
    Summary = "Gets hospital by rating",
    Description = "Gets all hospital with the specified rating",
    OperationId = "GetHospitalByRating"
)]
    [SwaggerResponse(200, "Hospitals were found", typeof(IEnumerable<HospitalResource>))]
    public async Task<ActionResult> GetHospitalsByRating(int rating)
    {
        var getHospitalByRatingQuery = new GetHospitalByRatingQuery(rating);
        var results = await hospitalQueryService.Handle(getHospitalByRatingQuery);

        if (results == null || !results.Any())
            return NotFound();

        var resources = results.Select(HospitalResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

}
