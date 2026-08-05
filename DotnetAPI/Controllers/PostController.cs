using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly DataContextDapper _dapper;

        public PostController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
        }

        [HttpGet("Posts")]
        public IEnumerable<Post> GetPosts()
        {
             string sql = @"
                SELECT 
                    [PostId]
                    , [UserId]
                    , [PostTitle]
                    , [PostContent]
                    , [PostCreated]
                    , [PostUpdated]
                FROM  TutorialAppSchema.Posts";
            IEnumerable<Post> posts = _dapper.LoadData<Post>(sql) ;
            return posts;
            
        }

        [HttpGet("Posts/{postId}")]
        public Post GetSinglePost(string postId)
        {
            string sql = @"
                SELECT  [PostId]
                    , [UserId]
                    , [PostTitle]
                    , [PostContent]
                    , [PostCreated]
                    , [PostUpdated]
                FROM  TutorialAppSchema.Posts  
                WHERE PostId = "+ postId.ToString();
            Post post = _dapper.LoadDataSingle<Post>(sql) ;
            return post;
        }

        [HttpGet("PostsByUser/{userId}")]
        public IEnumerable<Post> GetPostsByUser(string userId)
        {
            string sql = @"
                SELECT  [PostId]
                    , [UserId]
                    , [PostTitle]
                    , [PostContent]
                    , [PostCreated]
                    , [PostUpdated]
                FROM  TutorialAppSchema.Posts  
                WHERE UserId = "+ userId.ToString();
           
            return _dapper.LoadData<Post>(sql) ;;
        }

        [HttpGet("MyPosts")]
        public IEnumerable<Post> GetMyPosts()
        {
            string sql = @"
                SELECT  [PostId]
                    , [UserId]
                    , [PostTitle]
                    , [PostContent]
                    , [PostCreated]
                    , [PostUpdated]
                FROM  TutorialAppSchema.Posts  
                WHERE UserId = "+ this.User.FindFirst("userId")?.Value;

            return _dapper.LoadData<Post>(sql) ;;
        }

        [HttpPost("Post")]
        public IActionResult AddPost(PostToAddDto newPost)
        {
            string sql = @"
            INSERT INTO TutorialAppSchema.Posts 
                ([UserId]
                , [PostTitle]
                , [PostContent]
                , [PostCreated]
                , [PostUpdated]
                ) VALUES (
                '" + this.User.FindFirst("userId")?.Value + 
                "', '" + newPost.PostTitle + 
                "', '" + newPost.PostContent + 
                "', GETDATE(), GETDATE())";
            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Failed to create post");
        }

        [HttpPut("Post")]
        public IActionResult EditPost(PostToEditDto editPost)
        {
            string sql = @"
            UPDATE TutorialAppSchema.Posts 
                 SET [PostTitle] = '" +editPost.PostTitle + 
                "', [PostContent] = '" + editPost.PostContent + 
                @"', [PostUpdated] = GETDATE() 
                WHERE PostId = " + editPost.PostId.ToString() +
                "AND UserId = " + this.User.FindFirst("userId")?.Value;
            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Failed to Update post");
        }

        [HttpDelete("Post")]
        public IActionResult DeletePost(int postId)
        {
            string sql = @"
            DELETE FROM TutorialAppSchema.Posts 
                WHERE PostId = " + postId.ToString() +
                "AND UserId = " + this.User.FindFirst("userId")?.Value;
            if (_dapper.ExecuteSql(sql))
            {
                return Ok();
            }
            throw new Exception("Failed to Delete post");
        }

        [HttpGet("PostsBySearch/{searchParam}")]
        public IEnumerable<Post> PostsBySearch(string searchParam)
        {
            string sql = @"
                SELECT  [PostId]
                    , [UserId]
                    , [PostTitle]
                    , [PostContent]
                    , [PostCreated]
                    , [PostUpdated]
                FROM  TutorialAppSchema.Posts  
                WHERE PostTitle LIKE '%"+searchParam + "%'" + 
                " OR [PostContent] LIKE '%" +searchParam + "%'";

            return _dapper.LoadData<Post>(sql) ;;
        }
    }
}