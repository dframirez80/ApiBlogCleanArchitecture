using Domain.Models.Dtos;
using Domain.Repository.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models.Profiles
{
    public class AppProfiles : Profile
    {
        public AppProfiles() {
            //CreateMap<OriginModel, DestinationModel>();
            CreateMap<ArticleDto, Article>();
            CreateMap<UserDto, User>();
            CreateMap<CommentDto, Comment>().ReverseMap();
        }
    }
}
