﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Pds.Api.Contracts;
using Pds.Api.Contracts.Bill;
using Pds.Api.Contracts.Client;
using Pds.Api.Contracts.Content;
using Pds.Api.Contracts.Cost;
using Pds.Api.Contracts.Person;
using Pds.Api.Contracts.Topic;
using Pds.Data.Entities;
using Pds.Services.Models;
using Pds.Services.Models.Client;
using Pds.Services.Models.Content;
using Pds.Services.Models.Cost;
using Pds.Web.Models.Content;

namespace Pds.Mappers
{
    public class ApiMappingProfile : Profile
    {
        public ApiMappingProfile()
        {
            #region Entities to Contracts

            CreateMap<Person, GetContentPersonDto>();
            CreateMap<Person, PersonDto>()
                .ForMember(
                    dest => dest.FullName,
                    opt => opt
                        .MapFrom(p => $"{p.LastName} {p.FirstName} {p.ThirdName}"))
                .ForMember(
                    dest => dest.Location,
                    opt => opt
                        .MapFrom(p => $"{p.Country} {p.City}"));
            CreateMap<Person, PersonForLookupDto>()
                .ForMember(
                    dest => dest.FullName,
                    opt => opt
                        .MapFrom((p,s) => 
                            s.FullName = string.IsNullOrEmpty(p.FirstName) ? 
                                "Не выбрано" : 
                                $"{p.FirstName} {p.ThirdName} {p.LastName}"));

            CreateMap<Resource, ResourceDto>();
            CreateMap<Resource, GetContentPersonResourceDto>();
            
            CreateMap<Content, BillContentDto>();
            CreateMap<Content, CostContentDto>();
            CreateMap<Content, PersonContentDto>();
            CreateMap<Content, ContentForLookupDto>()
                .ForMember(
                    dest => dest.Title,
                    opt => opt
                        .MapFrom((p, s) =>
                            s.Title = string.IsNullOrEmpty(p.Title) ? 
                                "Не выбрано" : 
                                $"{p.ReleaseDate:dd.MM} / {p.Title}"));
            CreateMap<Content, ContentDto>()
                .ForMember(
                    dest => dest.ClientName,
                    opt => opt
                        .MapFrom(p => p.Bill.Client.Name))
                .ForMember(
                    dest => dest.IsVisible,
                    opt => opt
                        .MapFrom(p => true));
            CreateMap<Content, GetContentResponse>();
            CreateMap<Content, GetContentForPayResponse>();

            CreateMap<Brand, Pds.Api.Contracts.BrandDto>();
            CreateMap<Brand, BrandForCheckboxesDto>();

            CreateMap<Client, ClientDto>();
            CreateMap<Client, GetClientResponse>();
            CreateMap<Client, GetContentBillClientDto>();
            CreateMap<Client, Pds.Api.Contracts.Content.ClientForLookupDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt
                        .MapFrom((p, s) =>
                            s.Name = p.Name ?? "Не выбрано"));
            CreateMap<Client, Pds.Api.Contracts.Bill.ClientForLookupDto>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt
                        .MapFrom((p, s) =>
                            s.Name = p.Name ?? "Не выбрано"));

            CreateMap<Bill, BillDto>();
            CreateMap<Bill, ClientBillDto>();
            CreateMap<Bill, GetContentBillDto>();
            CreateMap<Bill, ContentListBillDto>();
            CreateMap<Bill, GetContentBillForPayResponse>();

            CreateMap<Cost, CostDto>();
            CreateMap<Cost, GetCostResponse>();
            CreateMap<Cost, GetContentCostDto>();

            CreateMap<Brand, BrandDto>();

            CreateMap<Topic, TopicDto>();
            CreateMap<Topic, GetTopicResponse>();

            #endregion

            #region Contracts to Entities

            CreateMap<CreateClientRequest, Client>();
            CreateMap<CreateCostRequest, Cost>();
            CreateMap<CreateBillRequest, Bill>();
            CreateMap<ResourceDto, Resource>()
                .ForMember(
                    dest => dest.CreatedAt,
                    opt => opt
                        .MapFrom(p => DateTime.UtcNow));
            CreateMap<CreatePersonRequest, Person>()
                .ForMember(
                    dest => dest.Brands,
                    opt => opt
                        .MapFrom(p => BrandsDtoToBrandsCollection(p.Brands.Where( c => c.IsSelected).ToList())));
            
            CreateMap<CreateTopicRequest, Topic>()
                .ForMember(dest => dest.Persons,
                    opt =>
                        opt.MapFrom(ctr => 
                            ctr.People.Select(guid => new Person {Id = guid})));
            CreateMap<UpdateTopicRequest, Topic>()
                .ForMember(dest => dest.Persons,
                    opt =>
                        opt.MapFrom((ctr, _) => 
                            ctr.People.Select(guid => new Person {Id = guid})));

            #endregion
            
            #region Contracts to Models

            CreateMap<EditContentBillDto, EditContentBillModel>();
            CreateMap<EditContentRequest, EditContentModel>();
            CreateMap<EditClientRequest, EditClientModel>();
            CreateMap<EditCostRequest, EditCostModel>();
            CreateMap<CreateContentBillDto, CreateContentBillModel>();
            CreateMap<CreateContentRequest, CreateContentModel>()
                .ForMember(
                    dest => dest.BrandId,
                    opt => opt
                        .MapFrom(p => p.BrandId.Value));

            #endregion

            #region Contract to Contract

            CreateMap<GetContentResponse, EditContentRequest>();
            CreateMap<GetContentBillDto, EditContentBillDto>();
            CreateMap<GetCostResponse, EditCostRequest>();
            CreateMap<GetClientResponse, EditClientRequest>();
            CreateMap<Pds.Api.Contracts.BrandDto, Pds.Web.Models.Content.BrandFilterItem>();
            CreateMap<Pds.Api.Contracts.BrandDto, Pds.Web.Models.Bill.BrandFilterItem>();
            CreateMap<Pds.Api.Contracts.BrandDto, Pds.Web.Models.Cost.BrandFilterItem>();
            CreateMap<Pds.Api.Contracts.BrandDto, Pds.Web.Models.Person.BrandFilterItem>();

            #endregion

            #region Models to Entities

            CreateMap<EditContentBillModel, Bill>();

            #endregion
        }

        private ICollection<Brand> BrandsDtoToBrandsCollection(List<BrandForCheckboxesDto> brands)
        {
            return brands.Select(b => new Brand {Id = b.Id}).ToList();
        }
    }
}