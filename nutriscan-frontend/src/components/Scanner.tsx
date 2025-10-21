'use client';

import { useEffect, useRef, useState } from 'react';
import { BrowserMultiFormatReader } from 'zxing-js/browser';
import { useMutation } from '@tanstack/react-query';
import apiClient from '../lib/apiClient';
import { UserLogDTO } from '../types';

interface ScannerProps {
  initialBarcode?: string;
}

export default function Scanner({ initialBarcode }: ScannerProps) {
  const videoRef = useRef<HTMLVideoElement | null>(null);
  const [manualCode, setManualCode] = useState(initialBarcode ?? '');
  const [message, setMessage] = useState<string | null>(null);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const codeReader = new BrowserMultiFormatReader();

    if (!videoRef.current) return;

    codeReader
      .decodeFromVideoDevice(undefined, videoRef.current, (result, err) => {
        if (result) {
          setManualCode(result.getText());
          setMessage(`Código detectado: ${result.getText()}`);
        }
        if (err && !err.message.includes('No MultiFormat Readers')) {
          setError('Não foi possível ler o código.');
        }
      })
      .catch(() => {
        setError('Permita o acesso à câmera ou utilize o campo manual.');
      });

    return () => {
      codeReader.reset();
    };
  }, []);

  const mutation = useMutation({
    mutationFn: async (payload: UserLogDTO) => {
      const userId = process.env.NEXT_PUBLIC_DEMO_USER_ID ?? '11111111-1111-1111-1111-111111111111';
      await apiClient.post(`/api/users/${userId}/logs`, payload);
    },
    onSuccess: () => {
      setMessage('Log registrado com sucesso!');
      setError(null);
    },
    onError: () => {
      setError('Não foi possível registrar o log.');
    }
  });

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (!manualCode) {
      setError('Informe um código de barras válido.');
      return;
    }

    mutation.mutate({
      barcode: manualCode,
      quantity: 1,
      meal: 'snack'
    });
  };

  return (
    <div>
      <div className="ratio ratio-4x3 bg-dark rounded overflow-hidden mb-3">
        <video ref={videoRef} className="w-100 h-100" autoPlay muted playsInline />
      </div>
      <form onSubmit={handleSubmit} className="d-flex flex-column gap-3">
        <div>
          <label htmlFor="barcode" className="form-label">
            Código de barras
          </label>
          <input
            id="barcode"
            className="form-control"
            value={manualCode}
            onChange={(event) => setManualCode(event.target.value)}
            placeholder="Digite manualmente caso necessário"
          />
        </div>
        <button type="submit" className="btn btn-nutriscan" disabled={mutation.isPending}>
          {mutation.isPending ? 'Registrando...' : 'Adicionar ao meu dia'}
        </button>
      </form>
      {message && <p className="alert alert-success mt-3 mb-0">{message}</p>}
      {error && <p className="alert alert-danger mt-3 mb-0">{error}</p>}
    </div>
  );
}
