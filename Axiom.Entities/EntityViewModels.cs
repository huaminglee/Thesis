 
using System;
   
namespace Thesis.Entities.ViewModels
{
				
		public class ActivityViewModel
		{
			
			#region Properties
				 				
			public int ActivityID { get; set; }				
			 				
			public int TypeID { get; set; }				
			 				
			public int AddressID { get; set; }				
			 				
			public string Name { get; set; }				
			 				
			public string Description { get; set; }				
			 				
			public bool IsCompleted { get; set; }				
			 				
			public bool IsInvoiced { get; set; }				
			 				
			public DateTime? RemainderDate { get; set; }				
			 				
			public DateTime? StartDate { get; set; }				
			 				
			public DateTime EndDate { get; set; }				
			 				
			public int OwnerID { get; set; }				
			 				
			public int ExecuterID { get; set; }				
			 				
			public int? PlannedHours { get; set; }				
			 				
			public int? ShiftID { get; set; }				
			 				
			public decimal? Value { get; set; }				
			 				
			public string InvoiceText { get; set; }				
			 				
			public int? RelationID { get; set; }				
			 				
			public int? InvoiceAddressID { get; set; }				
			 				
			public int? DocumentID { get; set; }				
								
			#endregion

		}
			
							
		public class ActivityTypesViewModel
		{
			
			#region Properties
				 				
			public int ActivityTypeID { get; set; }				
			 				
			public string Name { get; set; }				
								
			#endregion

		}
			
							
		public class AddressesViewModel
		{
			
			#region Properties
				 				
			public int AddressID { get; set; }				
			 				
			public bool? IsActive { get; set; }				
			 				
			public int? AddressType { get; set; }				
			 				
			public string Street { get; set; }				
			 				
			public string HouseNumber { get; set; }				
			 				
			public string Addition { get; set; }				
			 				
			public string PostalCode { get; set; }				
			 				
			public int? CityID { get; set; }				
			 				
			public int? CountryID { get; set; }				
			 				
			public string Phone { get; set; }				
			 				
			public string Fax { get; set; }				
			 				
			public string Description { get; set; }				
			 				
			public string KeyPersonName { get; set; }				
			 				
			public string KeyPersonPhone { get; set; }				
			 				
			public int? ObjectType { get; set; }				
			 				
			public DateTime? LastInvoiceDate { get; set; }				
			 				
			public int? Electriciteit { get; set; }				
			 				
			public string DetailDescription { get; set; }				
			 				
			public string Location { get; set; }				
			 				
			public string Information { get; set; }				
			 				
			public string ExtraLetterText { get; set; }				
								
			#endregion

		}
			
							
		public class AddressTypesViewModel
		{
			
			#region Properties
				 				
			public int AddressTypeID { get; set; }				
			 				
			public string Name { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_ApplicationsViewModel
		{
			
			#region Properties
				 				
			public string ApplicationName { get; set; }				
			 				
			public string LoweredApplicationName { get; set; }				
			 				
			public Guid ApplicationId { get; set; }				
			 				
			public string Description { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_MembershipViewModel
		{
			
			#region Properties
				 				
			public Guid ApplicationId { get; set; }				
			 				
			public Guid UserId { get; set; }				
			 				
			public string Password { get; set; }				
			 				
			public int PasswordFormat { get; set; }				
			 				
			public string PasswordSalt { get; set; }				
			 				
			public string MobilePIN { get; set; }				
			 				
			public string Email { get; set; }				
			 				
			public string LoweredEmail { get; set; }				
			 				
			public string PasswordQuestion { get; set; }				
			 				
			public string PasswordAnswer { get; set; }				
			 				
			public bool IsApproved { get; set; }				
			 				
			public bool IsLockedOut { get; set; }				
			 				
			public DateTime CreateDate { get; set; }				
			 				
			public DateTime LastLoginDate { get; set; }				
			 				
			public DateTime LastPasswordChangedDate { get; set; }				
			 				
			public DateTime LastLockoutDate { get; set; }				
			 				
			public int FailedPasswordAttemptCount { get; set; }				
			 				
			public DateTime FailedPasswordAttemptWindowStart { get; set; }				
			 				
			public int FailedPasswordAnswerAttemptCount { get; set; }				
			 				
			public DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }				
			 				
			public string Comment { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_PathsViewModel
		{
			
			#region Properties
				 				
			public Guid ApplicationId { get; set; }				
			 				
			public Guid PathId { get; set; }				
			 				
			public string Path { get; set; }				
			 				
			public string LoweredPath { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_PersonalizationAllUsersViewModel
		{
			
			#region Properties
				 				
			public Guid PathId { get; set; }				
			 				
			public byte[] PageSettings { get; set; }				
			 				
			public DateTime LastUpdatedDate { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_PersonalizationPerUserViewModel
		{
			
			#region Properties
				 				
			public Guid Id { get; set; }				
			 				
			public Guid? PathId { get; set; }				
			 				
			public Guid? UserId { get; set; }				
			 				
			public byte[] PageSettings { get; set; }				
			 				
			public DateTime LastUpdatedDate { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_ProfileViewModel
		{
			
			#region Properties
				 				
			public Guid UserId { get; set; }				
			 				
			public string PropertyNames { get; set; }				
			 				
			public string PropertyValuesString { get; set; }				
			 				
			public byte[] PropertyValuesBinary { get; set; }				
			 				
			public DateTime LastUpdatedDate { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_RolesViewModel
		{
			
			#region Properties
				 				
			public Guid ApplicationId { get; set; }				
			 				
			public Guid RoleId { get; set; }				
			 				
			public string RoleName { get; set; }				
			 				
			public string LoweredRoleName { get; set; }				
			 				
			public string Description { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_SchemaVersionsViewModel
		{
			
			#region Properties
				 				
			public string Feature { get; set; }				
			 				
			public string CompatibleSchemaVersion { get; set; }				
			 				
			public bool IsCurrentVersion { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_UsersViewModel
		{
			
			#region Properties
				 				
			public Guid ApplicationId { get; set; }				
			 				
			public Guid UserId { get; set; }				
			 				
			public string UserName { get; set; }				
			 				
			public string LoweredUserName { get; set; }				
			 				
			public string MobileAlias { get; set; }				
			 				
			public bool IsAnonymous { get; set; }				
			 				
			public DateTime LastActivityDate { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_UsersInRolesViewModel
		{
			
			#region Properties
				 				
			public Guid UserId { get; set; }				
			 				
			public Guid RoleId { get; set; }				
								
			#endregion

		}
			
							
		public class aspnet_WebEvent_EventsViewModel
		{
			
			#region Properties
				 				
			public string EventId { get; set; }				
			 				
			public DateTime EventTimeUtc { get; set; }				
			 				
			public DateTime EventTime { get; set; }				
			 				
			public string EventType { get; set; }				
			 				
			public decimal EventSequence { get; set; }				
			 				
			public decimal EventOccurrence { get; set; }				
			 				
			public int EventCode { get; set; }				
			 				
			public int EventDetailCode { get; set; }				
			 				
			public string Message { get; set; }				
			 				
			public string ApplicationPath { get; set; }				
			 				
			public string ApplicationVirtualPath { get; set; }				
			 				
			public string MachineName { get; set; }				
			 				
			public string RequestUrl { get; set; }				
			 				
			public string ExceptionType { get; set; }				
			 				
			public string Details { get; set; }				
								
			#endregion

		}
			
							
		public class CitiesViewModel
		{
			
			#region Properties
				 				
			public int CityID { get; set; }				
			 				
			public int CountryID { get; set; }				
			 				
			public string Name { get; set; }				
			 				
			public string AreaCode { get; set; }				
								
			#endregion

		}
			
							
		public class CompaniesViewModel
		{
			
			#region Properties
				 				
			public int CompanyID { get; set; }				
			 				
			public string Name { get; set; }				
								
			#endregion

		}
			
							
		public class CountriesViewModel
		{
			
			#region Properties
				 				
			public int CountryID { get; set; }				
			 				
			public string Name { get; set; }				
			 				
			public string AreaCode { get; set; }				
			 				
			public string Flag { get; set; }				
								
			#endregion

		}
			
							
		public class ElectriciteitiesViewModel
		{
			
			#region Properties
				 				
			public int ElectriciteitID { get; set; }				
			 				
			public string Name { get; set; }				
								
			#endregion

		}
			
							
		public class EmployeesViewModel
		{
			
			#region Properties
				 				
			public int EmployeeId { get; set; }				
			 				
			public string Name { get; set; }				
			 				
			public string Surname { get; set; }				
			 				
			public DateTime? StartDate { get; set; }				
			 				
			public decimal? Salary { get; set; }				
			 				
			public int? Age { get; set; }				
								
			#endregion

		}
			
							
		public class FilesViewModel
		{
			
			#region Properties
				 				
			public int FileID { get; set; }				
			 				
			public string FileName { get; set; }				
			 				
			public string Mimetype { get; set; }				
			 				
			public int? Size { get; set; }				
			 				
			public string Path { get; set; }				
			 				
			public string Alias { get; set; }				
								
			#endregion

		}
			
							
		public class ItemsViewModel
		{
			
			#region Properties
				 				
			public int ItemId { get; set; }				
			 				
			public string ItemCode { get; set; }				
			 				
			public string Description { get; set; }				
								
			#endregion

		}
			
							
		public class LanguagesViewModel
		{
			
			#region Properties
				 				
			public int LanguageId { get; set; }				
			 				
			public string Name { get; set; }				
			 				
			public string Code { get; set; }				
								
			#endregion

		}
			
							
		public class LookupViewModel
		{
			
			#region Properties
				 				
			public int LookupId { get; set; }				
			 				
			public int LookupTypeId { get; set; }				
								
			#endregion

		}
			
							
		public class LookupResourcesViewModel
		{
			
			#region Properties
				 				
			public int LookupResourcesId { get; set; }				
			 				
			public int LookupId { get; set; }				
			 				
			public int LanguageId { get; set; }				
			 				
			public string Text { get; set; }				
								
			#endregion

		}
			
							
		public class ModulesViewModel
		{
			
			#region Properties
				 				
			public int ModuleId { get; set; }				
			 				
			public string Name { get; set; }				
			 				
			public string LoweredName { get; set; }				
								
			#endregion

		}
			
							
		public class ModulesInRolesViewModel
		{
			
			#region Properties
				 				
			public int ModulesInRolesId { get; set; }				
			 				
			public int ModuleId { get; set; }				
			 				
			public Guid RoleId { get; set; }				
			 				
			public int ProcessTypeId { get; set; }				
								
			#endregion

		}
			
							
		public class ObjectTypesViewModel
		{
			
			#region Properties
				 				
			public int ObjectTypeID { get; set; }				
			 				
			public string Name { get; set; }				
								
			#endregion

		}
			
							
		public class RelationInAddressesViewModel
		{
			
			#region Properties
				 				
			public int RelationInAddressesID { get; set; }				
			 				
			public int AddressID { get; set; }				
			 				
			public int RelationID { get; set; }				
								
			#endregion

		}
			
							
		public class RelationsViewModel
		{
			
			#region Properties
				 				
			public int RelationID { get; set; }				
			 				
			public string Name { get; set; }				
								
			#endregion

		}
			
							
		public class RolesInTreeViewModel
		{
			
			#region Properties
				 				
			public int RolesInTreeId { get; set; }				
			 				
			public Guid RoleId { get; set; }				
			 				
			public Guid ParentRoleId { get; set; }				
								
			#endregion

		}
			
							
		public class RoofsViewModel
		{
			
			#region Properties
				 				
			public int RoofID { get; set; }				
			 				
			public string Name { get; set; }				
								
			#endregion

		}
			
							
		public class ShiftsViewModel
		{
			
			#region Properties
				 				
			public int ShiftID { get; set; }				
			 				
			public string Name { get; set; }				
								
			#endregion

		}
			
							
		public class sysdiagramsViewModel
		{
			
			#region Properties
				 				
			public string name { get; set; }				
			 				
			public int principal_id { get; set; }				
			 				
			public int diagram_id { get; set; }				
			 				
			public int? version { get; set; }				
			 				
			public byte[] definition { get; set; }				
								
			#endregion

		}
			
			//End Of Operations	
}  
