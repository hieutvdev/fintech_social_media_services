using BuildingBlocks.Pagination;
using ShredKernel.BaseClasses;

namespace BuildingBlocks.Utilities.ParamExtensions;

public static class ParamConvert
{
   /// <summary>
   /// Converts a SearchListModel into a SearchBaseModel and PaginationRequest.
   /// </summary>
   /// <param name="model">The source SearchListModel to convert.</param>
   /// <returns>A tuple containing SearchBaseModel and PaginationRequest.</returns>
   public static (SearchBaseModel SearchModel, PaginationRequest Pagination) ParamConvertSearchModelList(SearchListModel model)
   {
      if (model == null)
      {
         throw new ArgumentNullException(nameof(model), "The model cannot be null.");
      }

      var searchBaseModel = new SearchBaseModel(
         sortBy: model.SortBy ?? "Id",
         isAscending: model.IsAscending,
         keySearch: model.KeySearch,
         startDate: model.StartDate,
         endDate: model.EndDate
      );

      var paginationRequest = new PaginationRequest(
         PageIndex: model.PageIndex ?? 1,
         PageSize: model.PageSize ?? 10
      );

      return (searchBaseModel, paginationRequest);
   }

   public static PaginationRequest ToPaginationRequest(this SearchListModel searchListModel)
   {
      return new PaginationRequest(
         PageIndex: searchListModel.PageIndex ?? 0,
         PageSize: searchListModel.PageSize ?? 10);
   }


}
