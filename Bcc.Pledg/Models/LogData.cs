﻿using System;
using System.ComponentModel;

namespace Bcc.Pledg.Models
{
    public class LogData
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string CityCodeKATO { get; set; }
        public string City { get; set; }
        public string SectorCode { get; set; }
        public string Sector { get; set; }
        public string RelativityLocation { get; set; }
        public string SectorDescription { get; set; }
        public string TypeEstateCode { get; set; }
        public string TypeEstateByRef { get; set; }
        public string ApartmentLayoutCode { get; set; }
        public string ApartmentLayout { get; set; }
        public int? WallMaterialCode { get; set; }
        public string WallMaterial { get; set; }
        public string DetailAreaCode { get; set; }
        public string DetailArea { get; set; }
        public int? MinCostPerSQM { get; set; }
        public int? MaxCostPerSQM { get; set; }
        public decimal? Bargain { get; set; }
        public int? MinCostWithBargain { get; set; }
        public int? MaxCostWithBargain { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Action { get; set; }
        public string Username { get; set; }
        public DateTime ChangeDate { get; set; }
        public char IsArch { get; set; }
        public int? RCNameCode { get; set; }
        public string RCName { get; set; }
        public string ActualAdress { get; set; }
        public string FinQualityLevelCode { get; set; }
        public string FinQualityLevel { get; set; }
        public string Type { get; set; }
        public char? TypeCode { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public int? ProduceYear { get; set; }
        public long? MarketCost { get; set; }
        public int? MaxPercentageDeviation { get; set; }
    }
}
