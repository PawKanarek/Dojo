using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Insys.Ipott.Tiles.Messages;
using Insys.Ipott.Tiles.Messages.AdvancedFilters;
using Newtonsoft.Json;

namespace FixForCache
{
    public interface IPlatformArgs
    {
        string PlatformCodename { get; set; }
    }
    
    public class FilterTilesV2Args : IPlatformArgs
    {
        /// <summary>Filter by tile types (OR)</summary>
        public List<string> TileTypes { get; set; }

        /// <summary>PlatformCodename of app making the request</summary>
        public string PlatformCodename { get; set; }

        /// <summary>Page number for pagination</summary>
        public int Page { get; set; }

        /// <summary>Items per page for pagination</summary>
        public int Limit { get; set; }

        /// <summary>Sorting definition</summary>
        public List<SortInfo> Sort { get; set; }

        /// <summary>Filter by products (OR)</summary>
        public List<string> OrProductCodenames { get; set; }

        /// <summary>Filter by categories (OR)</summary>
        public List<string> OrCategoryCodenames { get; set; }

        /// <summary>Filter by categories (AND)</summary>
        public List<string> AndCategoryCodenames { get; set; }

        /// <summary>Filter by collections (OR)</summary>
        public List<string> OrCollectionCodenames { get; set; }

        /// <summary>User token - required if IsPurchased is set</summary>
        public string UserToken { get; set; }

        /// <summary>
        /// Return only assets purchased by user (user token is required)
        /// </summary>
        public bool IsPurchased { get; set; }

        /// <summary>
        /// Return only assets purchased by user as TVOD/SVOD (if IsPurchased=true and user token is required)
        /// </summary>
        public Insys.Video.Contracts.Messages.ProductType? ProductType { get; set; }

        /// <summary>
        /// Filter by IsEpisode: true = return only episodes of series, false = return only non-series assets
        /// </summary>
        public bool? IsEpisode { get; set; }

        /// <summary>Replace all episodes with single series tile</summary>
        public bool? AggregateSeries { get; set; }

        /// <summary>Args for advanced filters</summary>
        public IEnumerable<AdvancedFilterInfo> AdvancedFilters { get; set; }
    }
    
    public class FilterTilesV2Response : ServiceResponse
    {
        /// <summary>List of filtered tiles</summary>
        public List<PurchasedTileResult> Tiles { get; set; }

        /// <summary>Pagination info</summary>
        public PaginationResult Pagination { get; set; }
    }

    public class PurchasedTileResult : BaseTileResult
    {
        /// <summary>Gets or sets available to parameter.</summary>
        /// <value>User availability date</value>
        public DateTimeOffset? AvailableTo { get; set; }
    }
    
    public class AdvancedFilterInfo : 
        IYearAdvancedFilterInfo,
        IAdvancedFilterInfo,
        ICountryAdvancedFilterInfo,
        ICodenamesAdvancedFilterInfo,
        ICategoryAdvancedFilterInfo,
        IPeopleAdvancedFilterInfo
    {
        public IEnumerable<string> Codenames { get; set; }

        public FilterType FilterType { get; set; }

        public string TypeCodename { get; set; }

        public string RoleCodename { get; set; }

        public int? From { get; set; }

        public int? To { get; set; }
    }
    public interface IYearAdvancedFilterInfo : IAdvancedFilterInfo
    {
        int? From { get; set; }

        int? To { get; set; }
    }
    public interface IPeopleAdvancedFilterInfo : ICodenamesAdvancedFilterInfo, IAdvancedFilterInfo
    {
        string RoleCodename { get; set; }
    }
    
    public interface ICategoryAdvancedFilterInfo : ICodenamesAdvancedFilterInfo, IAdvancedFilterInfo
    {
        string TypeCodename { get; set; }
    }
    
    public interface ICountryAdvancedFilterInfo : ICodenamesAdvancedFilterInfo, IAdvancedFilterInfo
    {
    }
    
    public interface ICodenamesAdvancedFilterInfo : IAdvancedFilterInfo
    {
        IEnumerable<string> Codenames { get; set; }
    }
    
    public interface IAdvancedFilterInfo
    {
        FilterType FilterType { get; set; }
    }

    public class BaseTileResult
    {
        /// <summary>Gets or sets the identifier.</summary>
        /// <value>The identifier.</value>
        [JsonProperty(PropertyName = "i")]
        public string Id { get; set; }

        /// <summary>Gets or sets the type.</summary>
        /// <value>The type.</value>
        [JsonProperty(PropertyName = "t")]
        public string Type { get; set; }

        /// <summary>Gets or sets the origin entity identifier.</summary>
        /// <value>The origin entity identifier.</value>
        [JsonProperty(PropertyName = "oi")]
        public int OriginEntityId { get; set; }

        /// <summary>Gets or sets the codename.</summary>
        /// <value>The codename.</value>
        [JsonProperty(PropertyName = "c")]
        public string Codename { get; set; }
    }

    [DataContract]
    [JsonObject(MemberSerialization.OptOut)]
    public abstract class ServiceResponse
    {
        /// <summary>Result of service invocation.</summary>
        [DataMember]
        public ServiceResponse.ServiceResult Result { get; set; }

        public ServiceResponse() => this.Result = new ServiceResponse.ServiceResult();

        /// <summary>Result of service invocation.</summary>
        [DataContract]
        public class ServiceResult
        {
            /// <summary>Indicates whether operation completed without errors.</summary>
            /// <returns>
            /// true if operation completed without errors; otherwise false;
            /// </returns>
            [DataMember]
            public bool Success { get; private set; }

            /// <summary>Error/Warning code - 0 if no error/warning.</summary>
            [DataMember]
            public int Code { get; private set; }

            /// <summary>
            /// Codename of message. Related to Code, empty if no error/warning.
            /// </summary>
            [DataMember]
            public string MessageCodename { get; private set; }

            /// <summary>
            /// Details of the message or custom message if MessageCodename is not provided.
            /// </summary>
            [DataMember]
            public string MessageDetails { get; private set; }

            /// <summary>Gets the display message.</summary>
            /// <value>The display message (message for end user).</value>
            [DataMember]
            public string DisplayMessage { get; set; }

            /// <summary>Error log id</summary>
            public string LogId { get; set; }

            public ServiceResult()
            {
            }

            public ServiceResult(
                bool success = false,
                int code = 0,
                string messageCodename = null,
                string messageDetails = null,
                string displayMessage = null)
            {
                this.Success = success;
                this.Code = code;
                this.MessageCodename = messageCodename;
                this.MessageDetails = messageDetails;
                this.DisplayMessage = displayMessage;
            }
        }

        public class PaginationResult
        {
            /// <summary>Gets or sets the page.</summary>
            /// <value>The page.</value>
            public int Page { get; set; }

            /// <summary>Gets or sets the total pages.</summary>
            /// <value>The total pages.</value>
            public int TotalPages { get; set; }

            /// <summary>Gets or sets the limit.</summary>
            /// <value>The limit.</value>
            public int Limit { get; set; }

            /// <summary>Gets or sets the total items.</summary>
            /// <value>The total items.</value>
            public int TotalItems { get; set; }
        }
    }
    
    public class SortInfo
    {
        public string Field { get; set; }

        public SortDirection Direction { get; set; }

        public bool IsRaw { get; set; }

        public bool ShouldUseCollation => this.Field == "Title";
    }
}

namespace Insys.Video.Contracts.Messages
{
    public enum ProductType
    {
        TVOD = 1,
        SVOD = 2,
        PlaybackTime = 3,
        Offer = 4,
        Coupon = 5,
        Additional = 6,
        NonOttProduct = 7,
    }
}


namespace Insys.Ipott.Tiles.Messages
{
    public enum SortDirection
    {
        Asc,
        Desc,
    }
}

namespace Insys.Ipott.Tiles.Messages.AdvancedFilters
{
    public enum FilterType
    {
        People,
        Category,
        Country,
        Year,
    }
}

