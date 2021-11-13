using Domain.Models.Dtos;
using Domain.Repository.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models.Mapping
{
    public class AppProfiles : Profile
    {
        public AppProfiles() {
            //CreateMap<OriginModel, DestinationModel>();
            CreateMap<ArticleDto, Article>();
            CreateMap<UserDto, User>();
            CreateMap<CommentDto, Comment>().ReverseMap();

            // No es necesario mapear IEnumerable<>, con mapear los elementos es suficiente.
            // Al existir el mapeo CommentDto => Comment, solo es necesario agregar el ReverseMap(), para tener el mapeo inverso
            //CreateMap<Comment, CommentDto>(); 
        }
    }
}
