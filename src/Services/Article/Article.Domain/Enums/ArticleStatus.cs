namespace Article.Domain.Enums;

public enum ArticleStatus
{
    Draft = 0,            // Nháp
    SubmittedForReview = 1, // Gửi duyệt
    RevisionRequested = 2, // Yêu cầu sửa
    NotApproved = 3,      // Không chấp thuận
    Approved = 4,         // Đã duyệt
    NotAcceptingRevisions = 5, // Không chấp nhận sửa
    RevisionApproved = 6, // Chấp thuận sửa
    RevisedSubmittedForReview = 7 // Đã sửa gửi duyệt
}