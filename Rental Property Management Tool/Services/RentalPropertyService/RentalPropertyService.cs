﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Rental_Property_Management_Tool.Data;
using Rental_Property_Management_Tool.Dtos.RentalProperty;
using Rental_Property_Management_Tool.Entities;
using Rental_Property_Management_Tool.ServiceResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rental_Property_Management_Tool.Services.RentalPropertyService
{
    public class RentalPropertyService : IRentalPropertyService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RentalPropertyService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _context = context;
        }


        public async Task<ServiceResponse<List<GetRentalPropertyDto>>> GetAllRentalProperties(int? pageNumber, int? pageSize, string? sortParametar, string? searchQuery)
        {
            var response = new ServiceResponse<List<GetRentalPropertyDto>>();
            var dbRentalProperties = await _context.RentalProperties.Where(r => r.IsDeleted == false).ToListAsync();

            if (searchQuery != null)
                dbRentalProperties = dbRentalProperties.Where((p => p.Name.ToLower().Contains(searchQuery.ToLower())
                || p.Name.ToLower().Contains(searchQuery.ToLower()))).ToList();

            if (sortParametar != null)
            {
                switch (sortParametar)
                {
                    case "RentalStart":
                        dbRentalProperties = dbRentalProperties.OrderBy(q => q.RentalStart).ToList();
                        break;
                    case "RentalEnd":
                        dbRentalProperties = dbRentalProperties.OrderBy(q => q.RentalEnd).ToList();
                        break;
                    default:
                        break;
                }
            }
         

            var currentPageNumber = pageNumber ?? 1;
            var currentPageSize = pageSize ?? 10;
            response.Data =_mapper.Map<List<GetRentalPropertyDto>>(dbRentalProperties.Skip((currentPageNumber - 1) * currentPageSize).Take(currentPageSize).ToList());
            return response;
        }
        public async Task<ServiceResponse<GetRentalPropertyDto>> GetRentalPropertyById(int id)
        {
            var serviceResponse = new ServiceResponse<GetRentalPropertyDto>();
            var dbRentalProperties = await _context.RentalProperties.Include(x=>x.Costs).Include(x=>x.Person).FirstOrDefaultAsync(r => r.Id == id);
            serviceResponse.Data = _mapper.Map<GetRentalPropertyDto>(dbRentalProperties);

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetRentalPropertyDto>>> AddRentalProperty(AddRentalPropertyDto newRentalProperty)
        {
            var serviceResponse = new ServiceResponse<List<GetRentalPropertyDto>>();
            RentalProperty rentalProperty = _mapper.Map<RentalProperty>(newRentalProperty);
            _context.RentalProperties.Add(rentalProperty);
            await _context.SaveChangesAsync();
            serviceResponse.Data = await _context.RentalProperties.Select(r => _mapper.Map<GetRentalPropertyDto>(r)).ToListAsync();

            return serviceResponse;

        }
        public async Task<ServiceResponse<GetRentalPropertyDto>> UpdateRentalProperty(UpdateRentalPropertyDto updatedRentalProperty)
        {
            var serviceResponse = new ServiceResponse<GetRentalPropertyDto>();
            try
            {
                RentalProperty rentalProperty = await _context.RentalProperties.FirstOrDefaultAsync(r => r.Id == updatedRentalProperty.Id);
                rentalProperty.Name = updatedRentalProperty.Name;
                rentalProperty.SquaresMeters = updatedRentalProperty.SquaresMeters;
                rentalProperty.Address = updatedRentalProperty.Address;
                rentalProperty.IsRented = updatedRentalProperty.IsRented;
                rentalProperty.Type = updatedRentalProperty.Type;
                rentalProperty.RentalStart = updatedRentalProperty.RentalStart;
                rentalProperty.RentalEnd = updatedRentalProperty.RentalEnd;
                rentalProperty.IsRented = updatedRentalProperty.IsRented;

                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetRentalPropertyDto>(rentalProperty);


            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;


            }
            return serviceResponse;

        }

        public async Task<ServiceResponse<List<GetRentalPropertyDto>>> DeleteRentalProperty(int id)
        {
            var serviceResponse = new ServiceResponse<List<GetRentalPropertyDto>>();
            try
            {
                RentalProperty rentalProperty = await _context.RentalProperties.FirstOrDefaultAsync(r => r.Id == id);
                rentalProperty.IsDeleted = true;
                await _context.SaveChangesAsync();
                serviceResponse.Data = _context.RentalProperties.Select(r => _mapper.Map<GetRentalPropertyDto>(r)).ToList();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
        public async Task<ServiceResponse<GetRentalPropertyAndPersonRentedDto>> RentPropertyToPerson(int rentalPropertyId, int personId)
        {
            var serviceResponse = new ServiceResponse<GetRentalPropertyAndPersonRentedDto>();
            try
            {
                Person person = await _context.Persons.FirstOrDefaultAsync(p => p.Id == personId);

                if (person.Id != personId)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Person with given name does not exist.";
                    return serviceResponse;
                }

                RentalProperty rentalProperty = await _context.RentalProperties
                    .FirstOrDefaultAsync(rp => rp.Id == rentalPropertyId && rp.IsRented == false);
                rentalProperty.Person = person;
                rentalProperty.IsRented = true;
                await _context.SaveChangesAsync();
                serviceResponse.Success = true;
                serviceResponse.Data = _mapper.Map<GetRentalPropertyAndPersonRentedDto>(rentalProperty);
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
                return serviceResponse;
            }
        }
    }
}
