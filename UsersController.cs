using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(DAL.UserInfo.Instance.GetCount());
        }
        [HttpPut("username")]
        public ActionResult getUser(string username)
        {
            var result = DAL.UserInfo.Instance.GetModel(username);
            if (result != null)
                return Ok(Result.Ok(result));
            else
                return Ok(Result.Err("�û���������"));
        }
        [HttpPost]
        public ActionResult Post([FromBody]Model.UserInfo users)
        {
            try
            {
                int n = DAL.UserInfo.Instance.Add(users);
                return Ok(Result.Ok("��ӳɹ�"));
            }
            catch(Exception ex)
            {
                if (ex.Message.ToLower().Contains("primary"))
                    return Ok(Result.Err("�û����Ѵ���"));
                else if (ex.Message.ToLower().Contains("null"))
                    return Ok(Result.Err("�û��������롢��ݲ���Ϊ��"));
                else
                    return Ok(Result.Err(ex.Message));
            }
        }
        [HttpPut]
        public ActionResult Put([FromBody]Model.UserInfo users)
        {
            try
            {
                var n = DAL.UserInfo.Instance.Update(users);
                if (n != 0)
                    return Ok(Result.Ok("�޸ĳɹ�"));
                else
                    return Ok(Result.Err("�û���������"));
            }
            catch(Exception ex)
            {
                if (ex.Message.ToLower().Contains("null"))
                    return Ok(Result.Err("���룬��ݲ���Ϊ��"));
                else
                    return Ok(Result.Err(ex.Message));
            }
        }

        [HttpDelete("{username}")]
        public ActionResult Delete(string username)
        {
            try
            {
                var n = DAL.UserInfo.Instance.Delete(username);
                if (n != 0)
                    return Ok(Result.Ok("ɾ���ɹ�"));
                else
                    return Ok(Result.Err("�û���������"));
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().Contains("foreign"))
                    return Ok(Result.Err("��������Ʒ���û�����ɾ��"));
                else
                    return Ok(Result.Err(ex.Message));
            }
        }
        [HttpPost("check")]
        public ActionResult UserCheck([FromBody]Model.UserInfo users)
        {
            try
            {
                var result = DAL.UserInfo.Instance.GetModel(users.userName);
                if (result == null)
                    return Ok(Result.Err("�û�������"));
                else if (result.passWord == users.passWord)
                {
                    if (result.type == "����Ա")
                    {
                        result.passWord = "******";
                        return Ok(Result.Ok("����Ա��¼�ɹ�", result));
                    }
                    else
                        return Ok(Result.Err("ֻ�й���Ա�ܹ������̨����"));

                }
                else
                    return Ok(Result.Err("�������"));
            }
            catch(Exception ex)
            {
                return Ok(Result.Err(ex.Message));
                
            }
        }
        [HttpPost("genCheck")]
        public ActionResult genUserCheck([FromBody]Model.UserInfo users) 
        {
            try
            {
                var result = DAL.UserInfo.Instance.GetModel(users.userName);
                if (result == null)
                {
                    return Ok(Result.Err("�û�������"));
                }
                else if (result.passWord == users.passWord)
                {
                    result.passWord = "**********";
                    return Ok(Result.Ok("��¼�ɹ�", result));
                }
                else
                    return Ok(Result.Err("�������"));

            }
            catch (Exception ex)
            {
                return Ok(Result.Err(ex.Message));
            }
        }

        [HttpPost("page")]
        public ActionResult getPage([FromBody] Model.Page page)
        {
            var result = DAL.UserInfo.Instance.GetPage(page);
            if (result.Count() == 0)
                return Ok(Result.Err("���ؼ�¼��Ϊ0"));
            else
                return Ok(Result.Ok(result));
        }
    }
}