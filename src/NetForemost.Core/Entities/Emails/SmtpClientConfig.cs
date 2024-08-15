/*
PURPOSE

WRITTEN
  08/07/2022 18:05:36 (NetForemost)
COPYRIGHT
  Copyright © 2021–2022 NaturalSlim. All Rights Reserved.
WARNING
  This software is copyrighted! Any use of this software or other software
  whose copyright is held by IntelliProp or any software derived from such
  software without the prior written consent of the copyright holder is a
  violation of federal law punishable by imprisonment, fine or both.
  IntelliProp will pay a reward of three thousand dollars ($3,000) for
  information leading to successful civil litigation or criminal conviction
  of anyone violating a copyright held by IntelliProp.
*/

namespace NetForemost.Core.Entities.Emails;

public class SmtpClientConfig
{
    public string Host { get; set; }
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public bool UseDefaultCredentials { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}