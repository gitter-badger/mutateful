﻿using Mutate4l.Cli;
using Mutate4l.Commands;
using Mutate4l.Dto;
using Mutate4l.IO;
using Mutate4l.Options;
using System.Linq;

namespace Mutate4l
{
    public static class ClipProcessor
    {
        public static Result ProcessChainedCommand(ChainedCommand chainedCommand)
        {
            Clip[] sourceClips = chainedCommand.SourceClips.Where(c => c.Notes.Count > 0).ToArray();
            if (sourceClips.Length < 1)
            {
                return new Result("Clips are empty - aborting.");
            }

            Clip[] currentSourceClips = sourceClips;
            ProcessResultArray<Clip> resultContainer = new ProcessResultArray<Clip>("No commands specified");
            foreach (var command in chainedCommand.Commands)
            {
                resultContainer = ProcessCommand(command, currentSourceClips);
                if (resultContainer.Success)
                    currentSourceClips = resultContainer.Result;
                else
                    break;
            }
            if (resultContainer.Success && resultContainer.Result.Length > 0)
            {
                UdpConnector.SetClipById(chainedCommand.TargetId, resultContainer.Result[0]);
            }
            else
                return new Result("No clips affected");

            return new Result(resultContainer.Success, resultContainer.ErrorMessage);
        }

        public static ProcessResultArray<Clip> ProcessCommand(Command command, Clip[] clips)
        {
            ProcessResultArray<Clip> resultContainer;
            switch (command.Id)
            {
                case TokenType.Interleave:
                    resultContainer = Interleave.Apply(OptionParser.ParseOptions<InterleaveOptions>(command), clips); 
                    break;
                case TokenType.Constrain:
                    resultContainer = Constrain.Apply(OptionParser.ParseOptions<ConstrainOptions>(command), clips);
                    break;
                case TokenType.Slice:
                    resultContainer = Slice.Apply(OptionParser.ParseOptions<SliceOptions>(command), clips);
                    break;
                case TokenType.Arpeggiate:
                    resultContainer = Arpeggiate.Apply(OptionParser.ParseOptions<ArpeggiateOptions>(command), clips);
                    break;
                case TokenType.Monophonize:
                    resultContainer = Monophonize.Apply(clips);
                    break;
                case TokenType.Ratchet:
                    resultContainer = Ratchet.Apply(OptionParser.ParseOptions<RatchetOptions>(command), clips);
                    break;
                case TokenType.Scan:
                    resultContainer = Scan.Apply(OptionParser.ParseOptions<ScanOptions>(command), clips);
                    break;
                case TokenType.Filter:
                    resultContainer = Filter.Apply(OptionParser.ParseOptions<FilterOptions>(command), clips);
                    break;
                default:
                    // todo: error here
                    return new ProcessResultArray<Clip>($"Unsupported command {command.Id}");
            }
            return resultContainer;
        }
    }
}
