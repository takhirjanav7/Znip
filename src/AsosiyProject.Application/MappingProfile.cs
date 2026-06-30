using AsosiyProject.Application.DTOs.CommentDTOs;
using AsosiyProject.Application.DTOs.FollowDTOs;
using AsosiyProject.Application.DTOs.LikeDTOs;
using AsosiyProject.Application.DTOs.MessageDTOs;
using AsosiyProject.Application.DTOs.NotificationDTOs;
using AsosiyProject.Application.DTOs.PostDTOs;
using AsosiyProject.Application.DTOs.UserDTOs;
using AsosiyProject.Application.Extensions;
using AsosiyProject.Application.SignUp.Registration;
using AsosiyProject.Domain.Entities;
using AutoMapper;

namespace AsosiyProject.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // === USER ===
        CreateMap<User, UserProfileDto>()
            .ForMember(dest => dest.FollowersCount, opt => opt.MapFrom(src => src.Followers.Count))
            .ForMember(dest => dest.FollowingCount, opt => opt.MapFrom(src => src.Following.Count))
            .ForMember(dest => dest.PostsCount, opt => opt.MapFrom(src => src.Posts.Count));
            //.ForMember(dest => dest.ProjectsCount, opt => opt.MapFrom(src => src.Projects.Count));

        CreateMap<User, UserSmallDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.ProfilePictureUrl))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId));


        // === POST ===
        CreateMap<Post, GetPostDto>()
            .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User !=null ? src.User.UserName : "Unknown"))
            // .ForMember(dest => dest.UserAvatar, opt => opt.MapFrom(src => src.User.ProfilePictureUrl)) // Agar DTO da bo'lsa

            // Statistikalar (Bazadagi int fieldlardan)
            .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.LikeCount))
            .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.CommentCount))
            .ForMember(dest => dest.ViewCount, opt => opt.MapFrom(src => src.ViewCount))

            // !!! MUHIM: IsLikedByMe ni AutoMapperda qilmaymiz, chunki UserID dinamik.
            // Buni Service qismida to'ldiramiz.
            .ForMember(dest => dest.IsLikedByMe, opt => opt.Ignore())

            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

        // CreatePostDto -> Post (Entity)
        CreateMap<CreatePostDto, Post>()
            .ForMember(dest => dest.PostId, opt => opt.Ignore()) // ID avtomatik generatsiya qilinadi
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Likes, opt => opt.Ignore());
        
        // === COMMENT ===
        CreateMap<PostComment, GetCommentDto>()
            .ForMember(dest => dest.CommentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.PostId))
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))

            // Comment egasi haqida ma'lumot
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.UserName : ""))
            .ForMember(dest => dest.UserAvatar, opt => opt.MapFrom(src => src.User != null ? src.User.ProfilePictureUrl : null));

        CreateMap<UpdatePostDto, Post>()
            .ForMember(dest => dest.Caption, opt => opt.MapFrom(src => src.Caption))
            // Agar DTO da Id bo'lsa ham, uni o'tkazma degan ma'noda:
            .ForMember(dest => dest.PostId, opt => opt.Ignore())
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        // Like Entity -> PostLikerDto (Kim like bosdi ro'yxati uchun)
        // Bu yerda biz Like jadvalidan User ma'lumotlarini tortib olamiz
        CreateMap<Like, PostLikerDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.User.FullName))
            .ForMember(dest => dest.AvatarUrl, opt => opt.MapFrom(src => src.User.ProfilePictureUrl))

            // "IsFollowing" ni AutoMapper bilmaydi (chunki hozirgi user kimligini bilmaydi).
            // Buni Service qatlamida to'ldirish kerak.
            .ForMember(dest => dest.IsFollowing, opt => opt.Ignore());

        CreateMap<CreateCommentDto, PostComment>()
            .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text));


        // === SKILL ===
        CreateMap<Skill, GetSkillDto>()
            .ForMember(dest => dest.UsersCount, opt => opt.MapFrom(src => src.Users.Count));

        CreateMap<UserSkill, GetSkillWithUsersDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Skill.Name))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Skill.Category))
            .ForMember(dest => dest.IconUrl, opt => opt.MapFrom(src => src.Skill.IconUrl));

        // === CHAT MESSAGE ===
        CreateMap<ChatMessage, ChatMessageResultDto>()
            .ForMember(dest => dest.MessageId, opt => opt.MapFrom(src => src.MessageId));

        // === NOTIFICATION ===
        CreateMap<Notification, NotificationDto>()
            .ForMember(dest => dest.Actor, opt => opt.MapFrom(src => src.ActorId))
            .ForMember(dest => dest.TimeAgo, opt => opt.MapFrom(src => src.CreatedAt.ToTimeAgo()));

        // 2. LIKE MAPPINGLARI (Umumiy)
        // ======================================================

        // Like Entity -> GetLikeDto (Record)
        CreateMap<Like, GetLikeDto>()
            .ConstructUsing((src, context) => new GetLikeDto(
                src.Id,
                context.Mapper.Map<UserSmallDto>(src.User), // Ichma-ich mapping (User -> UserSmallDto)
                src.LikedAt
            )); 

        // CreateLikeDto -> Like Entity
        // Izoh: CreateLikeDto odatda Service ichida qo'lda map qilingani ma'qul, 
        // chunki "Post" yoki "Project" ekanligini tekshirish kerak. 
        // Lekin AutoMapperda shunday qilish mumkin:
        CreateMap<CreateLikeDto, Like>()
            .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.TargetType == "Post" ? (Guid?)src.TargetId : null))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.LikedAt, opt => opt.Ignore());

        CreateMap<Follow, GetFollowDto>()
            .ForMember(dest => dest.Follower, opt => opt.MapFrom(src => src.Follower))
            .ForMember(dest => dest.Following, opt => opt.MapFrom(src => src.Following));
    }
}