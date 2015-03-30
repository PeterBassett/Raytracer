using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Raytracer.Rendering.FileTypes.ObjFile
{
    class BufferedStreamReader
    {
        private Stream _stream;
        private byte[] _buffer;        
        private int _bufferPopulatedTo;
        private int _readPosition;
        private const int BufferSize = 32768;

        private const int InitiallineBufferLength = 8;
        private int _lineBufferLength = InitiallineBufferLength;
        private byte[] _lineBuffer = new byte[InitiallineBufferLength];        

        public BufferedStreamReader(string file)
        {
            _stream = new FileStream(file, FileMode.Open, FileAccess.Read);
            _buffer = new byte[BufferSize];
            FillBuffer();
        }

        public string ReadLine()
        {
            int index = 0;
            while (true)
            {
                if(_readPosition > BufferSize - 1)
                    FillBuffer();

                if (_bufferPopulatedTo == 0)
                {
                    return Encoding.Default.GetString(_lineBuffer, 0, index);
                }

                var next = _buffer[_readPosition];

                if (!IsEndOfLineChar(next))
                {
                    ResizeLineBuffer(index);
                    _lineBuffer[index] = next;
                }
                else
                {
                    int advanceBytes = 2;

                    byte nextPlus1 = 0;

                    if (_readPosition + 1 >= BufferSize)
                    {
                        FillBuffer();
                        nextPlus1 = _buffer[_readPosition];
                        advanceBytes = 1;
                    }
                    else
                    {
                        nextPlus1 = _buffer[_readPosition + 1];
                        advanceBytes = 2;
                    }

                    if (_readPosition + 1 >= BufferSize || IsEndOfLine(next, nextPlus1))
                    {
                        _readPosition += advanceBytes;
                        return Encoding.Default.GetString(_lineBuffer, 0, index);
                    }                    
                }
                
                _readPosition++;
                index++;
            }
        }

        private bool IsEndOfLine(byte next, byte nextPlus1)
        {
            return (next == '\r' && nextPlus1 == '\n' || next == '\n' && nextPlus1 == '\r');
        }

        private bool IsEndOfLineChar(byte next)
        {
            return (next == '\r' || next == '\n');
        }

        private void ResizeLineBuffer(int requiredSize)
        {
            if (requiredSize < _lineBufferLength)
                return;

            var temp = new byte[_lineBufferLength * 2];

            Array.Copy(_lineBuffer, temp, _lineBufferLength);

            _lineBuffer = temp;
            _lineBufferLength = _lineBufferLength * 2;
        }

        private void FillBuffer()
        {
            _bufferPopulatedTo = _stream.Read(_buffer, 0, BufferSize);
            _readPosition = 0;
        }

        public bool EndOfStream
        {
            get
            {
                return (_bufferPopulatedTo <= _readPosition && _bufferPopulatedTo < BufferSize);
            }
        }
    }
}
