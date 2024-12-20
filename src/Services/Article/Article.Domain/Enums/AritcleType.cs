namespace Article.Domain.Enums;

public enum ArticleType
{
    Sample = 1,      // Bài viết mẫu, dùng để làm ví dụ hoặc bản nháp.
    Hot = 2,         // Bài viết nổi bật, đang được quan tâm nhiều hoặc đang thịnh hành.
    News = 3,        // Bài viết dạng tin tức, cập nhật các thông tin mới nhất.
    Blog = 4,        // Bài viết dạng blog cá nhân, chia sẻ quan điểm hoặc trải nghiệm.
    Review = 5,      // Bài viết đánh giá, thường là về sản phẩm, dịch vụ hoặc nội dung cụ thể.
    Tutorial = 6,    // Bài viết hướng dẫn, cung cấp quy trình hoặc cách làm chi tiết.
    Opinion = 7,     // Bài viết thể hiện quan điểm, ý kiến cá nhân của tác giả.
    Advertisement = 8, // Bài viết quảng cáo, thường nhằm mục đích thương mại hoặc tiếp thị.
    Other = 9        // Bài viết khác, không thuộc các loại trên hoặc không được phân loại cụ thể.
}