using NewsAggregatorApp.Entities;
using NewsAggregatorDTOs;
using Riok.Mapperly.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregatorMapper
{
    [Mapper]
    public static partial class SourceMapper
    {
        [MapProperty(nameof(Source.SourceId), nameof(SourceDto.SourceDtoId))]
        public static partial SourceDto? SourceToSourceDto(Source? source);

        [MapProperty(nameof(SourceDto.SourceDtoId), nameof(Source.SourceId))]
        public static partial Source? SourceDtoToSource(SourceDto? sourceDto);
    }
}
