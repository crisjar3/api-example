/*
PURPOSE

WRITTEN
  19/07/2022 19:18:15 (NetForemost)
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

using NetForemost.Core.Dtos.Countries;

namespace NetForemost.Core.Dtos.Cities;

public class CityDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string IsoCode { get; set; }

    public CountryDto Country { get; set; }
}