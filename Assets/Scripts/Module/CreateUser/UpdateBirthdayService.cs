using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using DataModel;

namespace Assets.Scripts.Module.CreateUser
{
    public class UpdateBirthdayService : RemoteService<UpdateUserBirthdayRes>
    {
        public UpdateBirthdayService SetBirthday(int month, int day)
        {
            UpdateUserBirthdayReq req = new UpdateUserBirthdayReq()
            {
                Day = day,
                Month = month
            };

            requstBytes = NetWorkManager.GetByteData(req);

            return this;
        }
        
        protected override void OnExecute()
        {
            InitData(CMD.UPDATE_USER_BIRTHDAY, requstBytes);
        }

        protected override void ProcessData()
        {
            GlobalData.PlayerModel.PlayerVo.Birthday = _data.User.Birthday;
        }
    }
}