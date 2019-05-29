using Arthur.App;
using Arthur.App.Comm;
using Arthur.App.Model;
using Arthur.Core;
using Arthur.Core.Transfer;
using GMCC.Sorter.Data;
using GMCC.Sorter.Run;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GMCC.Sorter.ViewModel
{
    public abstract class SerialCommorViewModel : CommorViewModel
    {

        public SerialCommorViewModel(Commor commor) : base(commor)
        {
            this.portName = ((SerialCommor)commor.Communicator).PortName;
            this.baudRate = ((SerialCommor)commor.Communicator).BaudRate;
            this.parity = ((SerialCommor)commor.Communicator).Parity;
            this.dataBits = ((SerialCommor)commor.Communicator).DataBits;
            this.stopBits = ((SerialCommor)commor.Communicator).StopBits;
        }

        private string portName;
        public string PortName
        {
            get
            {
                if(portName == null)
                {
                    portName = ((SerialCommor)this.Commor.Communicator).PortName;
                }
                return portName;
            }
            set
            {
                if (portName != value)
                {
                    using (var db = new Data.AppContext())
                    {
                        if (Current.TrayScaners.Contains(this))
                        {
                            db.TrayScaners.FirstOrDefault(o => o.Id == this.Id).PortName = value;
                        }
                        db.SaveChanges();
                    }
                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. {0}串口号: [{1}] 修改为 [{2}]", Name, portName, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref portName, value);
                }
            }
        }


        private int? baudRate;
        public int BaudRate
        {
            get
            {
                if (baudRate == null)
                {
                    baudRate = ((SerialCommor)this.Commor.Communicator).BaudRate;
                }
                return baudRate.Value;
            }
            set
            {
                if (baudRate != value)
                {
                    using (var db = new Data.AppContext())
                    {
                        if (Current.TrayScaners.Contains(this))
                        {
                            db.TrayScaners.FirstOrDefault(o => o.Id == this.Id).BaudRate = value;
                        }
                        db.SaveChanges();
                    }
                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. {0}波特率: [{1}] 修改为 [{2}]", Name, baudRate, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref baudRate, value);
                }
            }
        }

        private Parity? parity;
        public Parity Parity
        {
            get
            {
                if (parity == null)
                {
                    parity = ((SerialCommor)this.Commor.Communicator).Parity;
                }
                return parity.Value;
            }
            set
            {
                if (parity != value)
                {
                    using (var db = new Data.AppContext())
                    {
                        if (Current.TrayScaners.Contains(this))
                        {
                            db.TrayScaners.FirstOrDefault(o => o.Id == this.Id).Parity = value;
                        }
                        db.SaveChanges();
                    }
                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. {0}奇偶校验位: [{1}] 修改为 [{2}]", Name, parity, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref parity, value);
                }
            }
        }

        private int? dataBits;
        public int DataBits
        {
            get
            {
                if (dataBits == null)
                {
                    dataBits = ((SerialCommor)this.Commor.Communicator).DataBits;
                }
                return dataBits.Value;
            }
            set
            {
                if (dataBits != value)
                {
                    using (var db = new Data.AppContext())
                    {
                        if (Current.TrayScaners.Contains(this))
                        {
                            db.TrayScaners.FirstOrDefault(o => o.Id == this.Id).DataBits = value;
                        }
                        db.SaveChanges();
                    }
                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. {0}数据位: [{1}] 修改为 [{2}]", Name, dataBits, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref dataBits, value);
                }
            }
        }

        private StopBits? stopBits;
        public StopBits StopBits
        {
            get
            {
                if (stopBits == null)
                {
                    stopBits = ((SerialCommor)this.Commor.Communicator).StopBits;
                }
                return stopBits.Value;
            }
            set
            {
                if (stopBits != value)
                {
                    using (var db = new Data.AppContext())
                    {
                        if (Current.TrayScaners.Contains(this))
                        {
                            db.TrayScaners.FirstOrDefault(o => o.Id == this.Id).StopBits = value;
                        }
                        db.SaveChanges();
                    }
                    Arthur.App.Business.Logging.AddOplog(string.Format("设备管理. {0}停止位: [{1}] 修改为 [{2}]", Name, stopBits, value), Arthur.App.Model.OpType.编辑);
                    SetProperty(ref stopBits, value);
                }
            }
        }
    }
}
