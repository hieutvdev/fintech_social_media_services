﻿using Article.Application.DTOs.Response.ProcessingStep;

namespace Article.Application.DTOs.Response.ArticleRequestPublish;

public record ArticleReqPublishDetailResDto(ArticleRequestPublishResBaseDto ArticleRequestPublish, IEnumerable<ProcessingStepResByReqDto> ProcessingSteps);
