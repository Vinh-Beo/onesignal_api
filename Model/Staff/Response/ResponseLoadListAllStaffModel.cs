namespace S3WAN.PMS.API
{
    class ResponseLoadListAllStaffModel
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public double JoinTime { get; set; }
        public bool IsActivated { get; set; }
        public double CreateTime { get; set; }
        public double UpdateTime { get; set; }
    }
}