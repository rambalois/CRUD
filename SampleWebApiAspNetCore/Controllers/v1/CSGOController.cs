using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Services;
using SampleWebApiAspNetCore.Models;
using SampleWebApiAspNetCore.Repositories;
using System.Text.Json;

namespace SampleWebApiAspNetCore.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CSGOController : ControllerBase
    {
        private readonly ICSGORepository _CSGORepository;
        private readonly IMapper _mapper;
        private readonly ILinkService<CSGOController> _linkService;

        public CSGOController(
            ICSGORepository CSGORepository,
            IMapper mapper,
            ILinkService<CSGOController> linkService)
        {
            _CSGORepository = CSGORepository;
            _mapper = mapper;
            _linkService = linkService;
        }

        [HttpGet(Name = nameof(GetAllCSGO))]
        public ActionResult GetAllCSGO(ApiVersion version, [FromQuery] QueryParameters queryParameters)
        {
            List<CSGOEntity> CSGOItems = _CSGORepository.GetAll(queryParameters).ToList();

            var allItemCount = _CSGORepository.Count();

            var paginationMetadata = new
            {
                totalCount = allItemCount,
                pageSize = queryParameters.PageCount,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(allItemCount)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = _linkService.CreateLinksForCollection(queryParameters, allItemCount, version);
            var toReturn = CSGOItems.Select(x => _linkService.ExpandSingleCSGOItem(x, x.Id, version));

            return Ok(new
            {
                value = toReturn,
                links = links
            });
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetSingleCSGO))]
        public ActionResult GetSingleCSGO(ApiVersion version, int id)
        {
            CSGOEntity CSGOItem = _CSGORepository.GetSingle(id);

            if (CSGOItem == null)
            {
                return NotFound();
            }

            CSGODto item = _mapper.Map<CSGODto>(CSGOItem);

            return Ok(_linkService.ExpandSingleCSGOItem(item, item.Id, version));
        }

        [HttpPost(Name = nameof(AddCSGO))]
        public ActionResult<CSGODto> AddCSGO(ApiVersion version, [FromBody] CSGOCreateDto CSGOCreateDto)
        {
            if (CSGOCreateDto == null)
            {
                return BadRequest();
            }

            CSGOEntity toAdd = _mapper.Map<CSGOEntity>(CSGOCreateDto);

            _CSGORepository.Add(toAdd);

            if (!_CSGORepository.Save())
            {
                throw new Exception("Creating a CSGOitem failed on save.");
            }

            CSGOEntity newCSGOItem = _CSGORepository.GetSingle(toAdd.Id);
            CSGODto CSGODto = _mapper.Map<CSGODto>(newCSGOItem);

            return CreatedAtRoute(nameof(GetSingleCSGO),
                new { version = version.ToString(), id = newCSGOItem.Id },
                _linkService.ExpandSingleCSGOItem(CSGODto, CSGODto.Id, version));
        }

        [HttpPatch("{id:int}", Name = nameof(PartiallyUpdateCSGO))]
        public ActionResult<CSGODto> PartiallyUpdateCSGO(ApiVersion version, int id, [FromBody] JsonPatchDocument<CSGOUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            CSGOEntity existingEntity = _CSGORepository.GetSingle(id);

            if (existingEntity == null)
            {
                return NotFound();
            }

            CSGOUpdateDto CSGOUpdateDto = _mapper.Map<CSGOUpdateDto>(existingEntity);
            patchDoc.ApplyTo(CSGOUpdateDto);

            TryValidateModel(CSGOUpdateDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(CSGOUpdateDto, existingEntity);
            CSGOEntity updated = _CSGORepository.Update(id, existingEntity);

            if (!_CSGORepository.Save())
            {
                throw new Exception("Updating a CSGOitem failed on save.");
            }

            CSGODto CSGODto = _mapper.Map<CSGODto>(updated);

            return Ok(_linkService.ExpandSingleCSGOItem(CSGODto, CSGODto.Id, version));
        }

        [HttpDelete]
        [Route("{id:int}", Name = nameof(RemoveCSGO))]
        public ActionResult RemoveCSGO(int id)
        {
            CSGOEntity CSGOItem = _CSGORepository.GetSingle(id);

            if (CSGOItem == null)
            {
                return NotFound();
            }

            _CSGORepository.Delete(id);

            if (!_CSGORepository.Save())
            {
                throw new Exception("Deleting a CSGOitem failed on save.");
            }

            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}", Name = nameof(UpdateCSGO))]
        public ActionResult<CSGODto> UpdateCSGO(ApiVersion version, int id, [FromBody] CSGOUpdateDto CSGOUpdateDto)
        {
            if (CSGOUpdateDto == null)
            {
                return BadRequest();
            }

            var existingCSGOItem = _CSGORepository.GetSingle(id);

            if (existingCSGOItem == null)
            {
                return NotFound();
            }

            _mapper.Map(CSGOUpdateDto, existingCSGOItem);

            _CSGORepository.Update(id, existingCSGOItem);

            if (!_CSGORepository.Save())
            {
                throw new Exception("Updating a CSGOitem failed on save.");
            }

            CSGODto CSGODto = _mapper.Map<CSGODto>(existingCSGOItem);

            return Ok(_linkService.ExpandSingleCSGOItem(CSGODto, CSGODto.Id, version));
        }

        [HttpGet("GetRandomCSGO", Name = nameof(GetRandomCSGO))] 
        public ActionResult GetRandomCSGO()
        {
            ICollection<CSGOEntity> CSGOItems = _CSGORepository.GetRandomCSGO();

            IEnumerable<CSGODto> dtos = CSGOItems.Select(x => _mapper.Map<CSGODto>(x));

            var links = new List<LinkDto>();

            // self 
            links.Add(new LinkDto(Url.Link(nameof(GetRandomCSGO), null), "self", "GET"));

            return Ok(new
            {
                value = dtos,
                links = links
            });
        }
    }
}
