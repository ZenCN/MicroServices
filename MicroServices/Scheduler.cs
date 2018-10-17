using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MicroServices
{
    [CLSCompliant(false)]
    public class Scheduler
    {
        private static IScheduler scheduler;
        public static IScheduler Current
        {
            get
            {
                if (scheduler == null)
                {
                    scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
                }
                return scheduler;
            }
        }

        /// <summary>
        /// 启动程序
        /// </summary>
        public void Start()
        {
            Current.Start();
            task();
        }

        /// <summary>
        /// 停止程序
        /// </summary>
        public void Stop()
        {
            Current.Shutdown();
        }

        /// <summary>
        /// 重启程序
        /// </summary>
        public void Restart()
        {
            Current.Shutdown();
            scheduler = null;
            Current.Start();
        }

        /// <summary>
        /// 任务
        /// </summary>
        public void task()
        {
            Job<QueryJob>("query", "0/8 * * * * ? "); //corn规则  "
            //Job<QueryJob>("query", "0 15 9 * * ? ");
        }

        /// <summary>
        /// 初始化任务
        /// </summary>
        public void Job<T>(string uid, string cronExpression) where T : IJob
        {
            //初始化创建作业
            var jobuilder = JobBuilder.Create<T>();//约束类型为 Ijob
                                                   //根据ID 标识触发器 构建调度
            var job = jobuilder.WithIdentity(uid, uid).Build();

            var cron = (ICronTrigger)TriggerBuilder.Create() //初始化触发器
                                            .WithIdentity("trigger" + uid, "group" + uid)//标识触发器
                                            .StartNow()//触发第一次运行
                                            .WithCronSchedule(cronExpression)//触发规则
                                            .Build();//构建调度
            //将创建的类线程和规则关联
            Current.ScheduleJob(job, cron);
        }
    }
}