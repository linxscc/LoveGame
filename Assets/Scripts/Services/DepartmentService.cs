using Assets.Scripts.Module.NetWork;
using Com.Proto;
using DataModel;
using Framework.GalaSports.Core;

namespace Assets.Scripts.Services
{
    public class DepartmentService : ComboService<object, DepartmentRuleRes,MyDepartmentRes>
    {
        protected override void OnExecute()
        {
            AddServiceData(CMD.DEPARTMENTC_DEPARTMENT_RULE,null,true);
            AddServiceData(CMD.DEPARTMENTC_MY_DEPARTMENT);
        }

        protected override void ProcessData(DepartmentRuleRes resU, MyDepartmentRes resV)
        {
            GlobalData.DepartmentRule = resU;
            GlobalData.DepartmentData = new MyDepartmentData();
            GlobalData.DepartmentData.InitData(resV);
        }
    }
}