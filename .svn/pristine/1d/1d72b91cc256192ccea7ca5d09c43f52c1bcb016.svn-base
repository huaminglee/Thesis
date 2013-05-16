 
using System;
using Thesis.Common.Attributes;
   
namespace Thesis.Entities.MetadataTypes
{
				
		public class ActivityMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int ActivityID { get; set; }				
			 
			[ValidateRequired, ValidateID] 
			public int TypeID { get; set; }				
			 
			[ValidateRequired, ValidateID] 
			public int AddressID { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(255)] 
			public string Name { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(255)] 
			public string Description { get; set; }				
			 
			[ValidateRequired] 
			public bool IsCompleted { get; set; }				
			 
			[ValidateRequired] 
			public bool IsInvoiced { get; set; }				
			 
			 
			public DateTime? RemainderDate { get; set; }				
			 
			 
			public DateTime? StartDate { get; set; }				
			 
			[ValidateRequired, ValidateDateTime] 
			public DateTime EndDate { get; set; }				
			 
			[ValidateRequired, ValidateID] 
			public int OwnerID { get; set; }				
			 
			[ValidateRequired, ValidateID] 
			public int ExecuterID { get; set; }				
			 
			 
			public int? PlannedHours { get; set; }				
			 
			[ValidateID] 
			public int? ShiftID { get; set; }				
			 
			 
			public decimal? Value { get; set; }				
			 
			[ValidateStringLength(255)] 
			public string InvoiceText { get; set; }				
			 
			[ValidateID] 
			public int? RelationID { get; set; }				
			 
			[ValidateID] 
			public int? InvoiceAddressID { get; set; }				
			 
			 
			public int? DocumentID { get; set; }				
								
			#endregion

		}
			
							
		public class ActivityTypesMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int ActivityTypeID { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(255)] 
			public string Name { get; set; }				
								
			#endregion

		}
			
							
		public class AddressesMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int AddressID { get; set; }				
			 
			 
			public bool? IsActive { get; set; }				
			 
			[ValidateID] 
			public int? AddressType { get; set; }				
			 
			[ValidateStringLength(255)] 
			public string Street { get; set; }				
			 
			[ValidateStringLength(255)] 
			public string HouseNumber { get; set; }				
			 
			[ValidateStringLength(255)] 
			public string Addition { get; set; }				
			 
			[ValidateStringLength(255)] 
			public string PostalCode { get; set; }				
			 
			[ValidateID] 
			public int? CityID { get; set; }				
			 
			[ValidateID] 
			public int? CountryID { get; set; }				
			 
			[ValidateStringLength(255)] 
			public string Phone { get; set; }				
			 
			[ValidateStringLength(255)] 
			public string Fax { get; set; }				
			 
			[ValidateStringLength(255)] 
			public string Description { get; set; }				
			 
			[ValidateStringLength(255)] 
			public string KeyPersonName { get; set; }				
			 
			[ValidateStringLength(255)] 
			public string KeyPersonPhone { get; set; }				
			 
			[ValidateID] 
			public int? ObjectType { get; set; }				
			 
			 
			public DateTime? LastInvoiceDate { get; set; }				
			 
			[ValidateID] 
			public int? Electriciteit { get; set; }				
			 
			[ValidateStringLength(255)] 
			public string DetailDescription { get; set; }				
			 
			[ValidateStringLength(255)] 
			public string Location { get; set; }				
			 
			[ValidateStringLength(255)] 
			public string Information { get; set; }				
			 
			[ValidateStringLength(255)] 
			public string ExtraLetterText { get; set; }				
								
			#endregion

		}
			
							
		public class AddressTypesMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int AddressTypeID { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(255)] 
			public string Name { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_ApplicationsMD
		{
			
			#region Properties
				 
			[ValidateRequired, ValidateStringLength(256)] 
			public string ApplicationName { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(256)] 
			public string LoweredApplicationName { get; set; }				
			 
			[ValidateRequired] 
			public Guid ApplicationId { get; set; }				
			 
			[ValidateStringLength(256)] 
			public string Description { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_MembershipMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public Guid ApplicationId { get; set; }				
			 
			[ValidateRequired] 
			public Guid UserId { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(128)] 
			public string Password { get; set; }				
			 
			[ValidateRequired] 
			public int PasswordFormat { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(128)] 
			public string PasswordSalt { get; set; }				
			 
			[ValidateStringLength(16)] 
			public string MobilePIN { get; set; }				
			 
			[ValidateStringLength(256)] 
			public string Email { get; set; }				
			 
			[ValidateStringLength(256)] 
			public string LoweredEmail { get; set; }				
			 
			[ValidateStringLength(256)] 
			public string PasswordQuestion { get; set; }				
			 
			[ValidateStringLength(128)] 
			public string PasswordAnswer { get; set; }				
			 
			[ValidateRequired] 
			public bool IsApproved { get; set; }				
			 
			[ValidateRequired] 
			public bool IsLockedOut { get; set; }				
			 
			[ValidateRequired, ValidateDateTime] 
			public DateTime CreateDate { get; set; }				
			 
			[ValidateRequired, ValidateDateTime] 
			public DateTime LastLoginDate { get; set; }				
			 
			[ValidateRequired, ValidateDateTime] 
			public DateTime LastPasswordChangedDate { get; set; }				
			 
			[ValidateRequired, ValidateDateTime] 
			public DateTime LastLockoutDate { get; set; }				
			 
			[ValidateRequired] 
			public int FailedPasswordAttemptCount { get; set; }				
			 
			[ValidateRequired, ValidateDateTime] 
			public DateTime FailedPasswordAttemptWindowStart { get; set; }				
			 
			[ValidateRequired] 
			public int FailedPasswordAnswerAttemptCount { get; set; }				
			 
			[ValidateRequired, ValidateDateTime] 
			public DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }				
			 
			[ValidateStringLength(1073741823)] 
			public string Comment { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_PathsMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public Guid ApplicationId { get; set; }				
			 
			[ValidateRequired] 
			public Guid PathId { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(256)] 
			public string Path { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(256)] 
			public string LoweredPath { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_PersonalizationAllUsersMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public Guid PathId { get; set; }				
			 
			[ValidateRequired] 
			public byte[] PageSettings { get; set; }				
			 
			[ValidateRequired, ValidateDateTime] 
			public DateTime LastUpdatedDate { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_PersonalizationPerUserMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public Guid Id { get; set; }				
			 
			 
			public Guid? PathId { get; set; }				
			 
			 
			public Guid? UserId { get; set; }				
			 
			[ValidateRequired] 
			public byte[] PageSettings { get; set; }				
			 
			[ValidateRequired, ValidateDateTime] 
			public DateTime LastUpdatedDate { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_ProfileMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public Guid UserId { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(1073741823)] 
			public string PropertyNames { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(1073741823)] 
			public string PropertyValuesString { get; set; }				
			 
			[ValidateRequired] 
			public byte[] PropertyValuesBinary { get; set; }				
			 
			[ValidateRequired, ValidateDateTime] 
			public DateTime LastUpdatedDate { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_RolesMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public Guid ApplicationId { get; set; }				
			 
			[ValidateRequired] 
			public Guid RoleId { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(256)] 
			public string RoleName { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(256)] 
			public string LoweredRoleName { get; set; }				
			 
			[ValidateStringLength(256)] 
			public string Description { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_SchemaVersionsMD
		{
			
			#region Properties
				 
			[ValidateRequired, ValidateStringLength(128)] 
			public string Feature { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(128)] 
			public string CompatibleSchemaVersion { get; set; }				
			 
			[ValidateRequired] 
			public bool IsCurrentVersion { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_UsersMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public Guid ApplicationId { get; set; }				
			 
			[ValidateRequired] 
			public Guid UserId { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(256)] 
			public string UserName { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(256)] 
			public string LoweredUserName { get; set; }				
			 
			[ValidateStringLength(16)] 
			public string MobileAlias { get; set; }				
			 
			[ValidateRequired] 
			public bool IsAnonymous { get; set; }				
			 
			[ValidateRequired, ValidateDateTime] 
			public DateTime LastActivityDate { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_UsersInRolesMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public Guid UserId { get; set; }				
			 
			[ValidateRequired] 
			public Guid RoleId { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_WebEvent_EventsMD
		{
			
			#region Properties
				 
			[ValidateRequired, ValidateStringLength(32)] 
			public string EventId { get; set; }				
			 
			[ValidateRequired, ValidateDateTime] 
			public DateTime EventTimeUtc { get; set; }				
			 
			[ValidateRequired, ValidateDateTime] 
			public DateTime EventTime { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(256)] 
			public string EventType { get; set; }				
			 
			[ValidateRequired] 
			public decimal EventSequence { get; set; }				
			 
			[ValidateRequired] 
			public decimal EventOccurrence { get; set; }				
			 
			[ValidateRequired] 
			public int EventCode { get; set; }				
			 
			[ValidateRequired] 
			public int EventDetailCode { get; set; }				
			 
			[ValidateStringLength(1024)] 
			public string Message { get; set; }				
			 
			[ValidateStringLength(256)] 
			public string ApplicationPath { get; set; }				
			 
			[ValidateStringLength(256)] 
			public string ApplicationVirtualPath { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(256)] 
			public string MachineName { get; set; }				
			 
			[ValidateStringLength(1024)] 
			public string RequestUrl { get; set; }				
			 
			[ValidateStringLength(256)] 
			public string ExceptionType { get; set; }				
			 
			[ValidateStringLength(1073741823)] 
			public string Details { get; set; }				
								
			#endregion

		}
			
							
		public class CitiesMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int CityID { get; set; }				
			 
			[ValidateRequired, ValidateID] 
			public int CountryID { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(255)] 
			public string Name { get; set; }				
			 
			[ValidateStringLength(10)] 
			public string AreaCode { get; set; }				
								
			#endregion

		}
			
							
		public class CompaniesMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int CompanyID { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(255)] 
			public string Name { get; set; }				
								
			#endregion

		}
			
							
		public class CountriesMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int CountryID { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(255)] 
			public string Name { get; set; }				
			 
			[ValidateStringLength(50)] 
			public string AreaCode { get; set; }				
			 
			[ValidateStringLength(150)] 
			public string Flag { get; set; }				
								
			#endregion

		}
			
							
		public class ElectriciteitiesMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int ElectriciteitID { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(255)] 
			public string Name { get; set; }				
								
			#endregion

		}
			
							
		public class EmployeesMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int EmployeeId { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(50)] 
			public string Name { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(50)] 
			public string Surname { get; set; }				
			 
			 
			public DateTime? StartDate { get; set; }				
			 
			 
			public decimal? Salary { get; set; }				
			 
			 
			public int? Age { get; set; }				
								
			#endregion

		}
			
							
		public class FilesMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int FileID { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(255)] 
			public string FileName { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(255)] 
			public string Mimetype { get; set; }				
			 
			 
			public int? Size { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(255)] 
			public string Path { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(255)] 
			public string Alias { get; set; }				
								
			#endregion

		}
			
							
		public class ItemsMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int ItemId { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(50)] 
			public string ItemCode { get; set; }				
			 
			[ValidateStringLength(50)] 
			public string Description { get; set; }				
								
			#endregion

		}
			
							
		public class LanguagesMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int LanguageId { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(50)] 
			public string Name { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(50)] 
			public string Code { get; set; }				
								
			#endregion

		}
			
							
		public class LookupMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int LookupId { get; set; }				
			 
			[ValidateRequired] 
			public int LookupTypeId { get; set; }				
								
			#endregion

		}
			
							
		public class LookupResourcesMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int LookupResourcesId { get; set; }				
			 
			[ValidateRequired, ValidateID] 
			public int LookupId { get; set; }				
			 
			[ValidateRequired, ValidateID] 
			public int LanguageId { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(200)] 
			public string Text { get; set; }				
								
			#endregion

		}
			
							
		public class ModulesMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int ModuleId { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(255)] 
			public string Name { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(255)] 
			public string LoweredName { get; set; }				
								
			#endregion

		}
			
							
		public class ModulesInRolesMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int ModulesInRolesId { get; set; }				
			 
			[ValidateRequired, ValidateID] 
			public int ModuleId { get; set; }				
			 
			[ValidateRequired] 
			public Guid RoleId { get; set; }				
			 
			[ValidateRequired] 
			public int ProcessTypeId { get; set; }				
								
			#endregion

		}
			
							
		public class ObjectTypesMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int ObjectTypeID { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(255)] 
			public string Name { get; set; }				
								
			#endregion

		}
			
							
		public class RelationInAddressesMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int RelationInAddressesID { get; set; }				
			 
			[ValidateRequired, ValidateID] 
			public int AddressID { get; set; }				
			 
			[ValidateRequired, ValidateID] 
			public int RelationID { get; set; }				
								
			#endregion

		}
			
							
		public class RelationsMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int RelationID { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(255)] 
			public string Name { get; set; }				
								
			#endregion

		}
			
							
		public class RolesInTreeMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int RolesInTreeId { get; set; }				
			 
			[ValidateRequired] 
			public Guid RoleId { get; set; }				
			 
			[ValidateRequired] 
			public Guid ParentRoleId { get; set; }				
								
			#endregion

		}
			
							
		public class RoofsMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int RoofID { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(255)] 
			public string Name { get; set; }				
								
			#endregion

		}
			
							
		public class ShiftsMD
		{
			
			#region Properties
				 
			[ValidateRequired] 
			public int ShiftID { get; set; }				
			 
			[ValidateRequired, ValidateStringLength(255)] 
			public string Name { get; set; }				
								
			#endregion

		}
			
							
		public class sysdiagramsMD
		{
			
			#region Properties
				 
			[ValidateRequired, ValidateStringLength(128)] 
			public string name { get; set; }				
			 
			[ValidateRequired] 
			public int principal_id { get; set; }				
			 
			[ValidateRequired] 
			public int diagram_id { get; set; }				
			 
			 
			public int? version { get; set; }				
			 
			 
			public byte[] definition { get; set; }				
								
			#endregion

		}
			
			//End Of Operations	
}  
