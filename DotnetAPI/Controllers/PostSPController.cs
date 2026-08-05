using DotnetAPI.Data;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostSPController : ControllerBase
    {
        private readonly DataContextDapper _dapper;

        public PostSPController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet("Posts/{postId}/{userId}/{searchParam}")]
        public IEnumerable<Post> GetPosts(int postId = 0 ,int userId = 0, string searchParam ="None")
        {
            string sql = @"EXEC TutorialAppSchema.spPosts_Get";
            string parameters = "";
            if(postId != 0)
            {
                parameters += ", @PostId="+ postId.ToString();
            }
            if(postId != 0)
            {
                parameters += ", @UserId="+ userId.ToString();
            }
            if(searchParam != "None")
            {
                parameters += ", @SearchValue='"+ searchParam+"'";
            }
            if(parameters.Length > 0)
            {
                sql += parameters.Substring(1); 
            }
            IEnumerable<Post> posts = _dapper.LoadData<Post>(sql) ;
            return posts;
        }

        [HttpGet("MyPosts")]
        public IEnumerable<Post> GetMyPosts()
        {
            string sql = @"EXEC TutorialAppSchema.spPosts_Get 
                @UserId = "+ this.User.FindFirst("userId")?.Value;

            return _dapper.LoadData<Post>(sql) ;;
        }

        [HttpPut("UpsertPost")]
        public IActionResult AddPost(Post upsertPost)
        {
            string sql = @"EXEC TutorialAppSchema.spPosts_Upsert
                @UserId = " + this.User.FindFirst("userId")?.Value + 
                ", @PostTitle ='" + upsertPost.PostTitle + 
                "', @PostContent = '" + upsertPost.PostContent + "'";
            if (upsertPost.PostId >0)
            {
                sql += ", @PostId = " + upsertPost.PostId;
            }
            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Failed to upsert post");
        }

        [HttpDelete("Post")]
        public IActionResult DeletePost(int postId)
        {
            string sql = @"EXEC TutorialAppSchema.spPost_Delete
                @PostId = " + postId.ToString() +
                ", @UserId = " + this.User.FindFirst("userId")?.Value;
            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Failed to Delete post");
        }
    }
}