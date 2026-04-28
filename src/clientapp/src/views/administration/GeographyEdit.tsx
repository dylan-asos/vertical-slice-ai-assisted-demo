import React, { useState, useEffect } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import {
  CCard,
  CCardBody,
  CCardHeader,
  CFormInput,
  CFormLabel,
  CFormSelect,
  CRow,
  CCol,
  CButton,
  CSpinner,
  CAlert,
  CFormText,
} from '@coreui/react'
import CIcon from '@coreui/icons-react'
import { cilGlobeAlt, cilSave, cilX } from '@coreui/icons'
import { useVerticalSliceClient, getGeographyById, upsertGeography } from '../../api'
import type { UpsertGeographyCommand } from '../../api'

const REGIONS = ['Americas', 'Europe', 'Asia', 'Africa', 'Oceania']

const SUB_REGIONS: Record<string, string[]> = {
  Americas: ['Northern America', 'Central America', 'South America', 'Caribbean'],
  Europe: ['Northern Europe', 'Western Europe', 'Eastern Europe', 'Southern Europe'],
  Asia: ['Eastern Asia', 'South-Eastern Asia', 'Southern Asia', 'Western Asia', 'Central Asia'],
  Africa: ['Northern Africa', 'Sub-Saharan Africa', 'Eastern Africa', 'Western Africa'],
  Oceania: ['Australia and New Zealand', 'Melanesia', 'Micronesia', 'Polynesia'],
}

const GeographyEdit = () => {
  useVerticalSliceClient()

  const navigate = useNavigate()
  const { id } = useParams<{ id: string }>()
  const isNew = !id || id === 'new'

  const [loading, setLoading] = useState(!isNew)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const [success, setSuccess] = useState<string | null>(null)

  const [form, setForm] = useState<UpsertGeographyCommand>({
    geographyId: undefined,
    name: '',
    shortCode: '',
    geoCodes: '',
    region: '',
    subRegion: '',
    capital: '',
    population: undefined,
    areaKm2: undefined,
  })

  useEffect(() => {
    if (!isNew && id) {
      loadGeography(Number(id))
    }
  }, [id, isNew])

  const loadGeography = async (geographyId: number) => {
    try {
      setLoading(true)
      setError(null)
      const response = await getGeographyById(geographyId)
      const geo = response.data

      setForm({
        geographyId: geo.geographyId,
        name: geo.name ?? '',
        shortCode: geo.shortCode ?? '',
        geoCodes: geo.geoCodes ?? '',
        region: geo.region ?? '',
        subRegion: geo.subRegion ?? '',
        capital: geo.capital ?? '',
        population: geo.population ?? undefined,
        areaKm2: geo.areaKm2 ?? undefined,
      })
    } catch (err) {
      console.error('Failed to load geography:', err)
      setError('Failed to load geography record')
    } finally {
      setLoading(false)
    }
  }

  const handleChange = (
    field: keyof UpsertGeographyCommand,
    value: string | number | undefined,
  ) => {
    setForm((prev) => ({ ...prev, [field]: value }))
    if (field === 'region') {
      setForm((prev) => ({ ...prev, region: value as string, subRegion: '' }))
    }
  }

  const handleSave = async () => {
    if (!form.name?.trim()) {
      setError('Name is required')
      return
    }
    if (!form.shortCode?.trim()) {
      setError('Short code is required')
      return
    }

    try {
      setSaving(true)
      setError(null)
      setSuccess(null)

      await upsertGeography(form)

      setSuccess(isNew ? 'Geography created successfully!' : 'Geography updated successfully!')
      setTimeout(() => navigate('/administration/geographies'), 1200)
    } catch (err) {
      console.error('Failed to save geography:', err)
      setError('Failed to save geography. Please check your input and try again.')
    } finally {
      setSaving(false)
    }
  }

  if (loading) {
    return (
      <CCard>
        <CCardBody className="text-center">
          <CSpinner size="lg" />
          <p className="mt-3">Loading geography...</p>
        </CCardBody>
      </CCard>
    )
  }

  const availableSubRegions = form.region ? (SUB_REGIONS[form.region] ?? []) : []

  return (
    <div>
      <CCard>
        <CCardHeader>
          <div className="d-flex justify-content-between align-items-center">
            <div className="d-flex align-items-center">
              <CIcon icon={cilGlobeAlt} className="me-2" />
              <h5 className="mb-0">{isNew ? 'Add Geography' : 'Edit Geography'}</h5>
            </div>
            <CButton
              color="secondary"
              variant="outline"
              size="sm"
              onClick={() => navigate('/administration/geographies')}
            >
              <CIcon icon={cilX} className="me-1" />
              Cancel
            </CButton>
          </div>
        </CCardHeader>
        <CCardBody>
          {error && (
            <CAlert color="danger" className="mb-3" dismissible onClose={() => setError(null)}>
              {error}
            </CAlert>
          )}
          {success && (
            <CAlert color="success" className="mb-3">
              {success}
            </CAlert>
          )}

          <CRow className="mb-3">
            <CCol md={6}>
              <CFormLabel htmlFor="name">
                Name <span className="text-danger">*</span>
              </CFormLabel>
              <CFormInput
                id="name"
                type="text"
                placeholder="e.g. United States"
                value={form.name ?? ''}
                onChange={(e) => handleChange('name', e.target.value)}
              />
            </CCol>
            <CCol md={3}>
              <CFormLabel htmlFor="shortCode">
                Short Code <span className="text-danger">*</span>
              </CFormLabel>
              <CFormInput
                id="shortCode"
                type="text"
                placeholder="e.g. US"
                maxLength={10}
                value={form.shortCode ?? ''}
                onChange={(e) => handleChange('shortCode', e.target.value.toUpperCase())}
              />
              <CFormText>ISO 3166-1 alpha-2 code</CFormText>
            </CCol>
            <CCol md={3}>
              <CFormLabel htmlFor="capital">Capital</CFormLabel>
              <CFormInput
                id="capital"
                type="text"
                placeholder="e.g. Washington, D.C."
                value={form.capital ?? ''}
                onChange={(e) => handleChange('capital', e.target.value)}
              />
            </CCol>
          </CRow>

          <CRow className="mb-3">
            <CCol md={4}>
              <CFormLabel htmlFor="region">Region</CFormLabel>
              <CFormSelect
                id="region"
                value={form.region ?? ''}
                onChange={(e) => handleChange('region', e.target.value)}
              >
                <option value="">Select region...</option>
                {REGIONS.map((r) => (
                  <option key={r} value={r}>
                    {r}
                  </option>
                ))}
              </CFormSelect>
            </CCol>
            <CCol md={4}>
              <CFormLabel htmlFor="subRegion">Sub-Region</CFormLabel>
              <CFormSelect
                id="subRegion"
                value={form.subRegion ?? ''}
                onChange={(e) => handleChange('subRegion', e.target.value)}
                disabled={availableSubRegions.length === 0}
              >
                <option value="">Select sub-region...</option>
                {availableSubRegions.map((sr) => (
                  <option key={sr} value={sr}>
                    {sr}
                  </option>
                ))}
              </CFormSelect>
            </CCol>
            <CCol md={4}>
              <CFormLabel htmlFor="geoCodes">Geo Codes (JSON)</CFormLabel>
              <CFormInput
                id="geoCodes"
                type="text"
                placeholder='e.g. {"latitude":37.09,"longitude":-95.71}'
                value={form.geoCodes ?? ''}
                onChange={(e) => handleChange('geoCodes', e.target.value)}
              />
              <CFormText>Geographic coordinates in JSON format</CFormText>
            </CCol>
          </CRow>

          <CRow className="mb-4">
            <CCol md={4}>
              <CFormLabel htmlFor="population">Population</CFormLabel>
              <CFormInput
                id="population"
                type="number"
                placeholder="e.g. 331000000"
                value={form.population ?? ''}
                onChange={(e) =>
                  handleChange('population', e.target.value ? Number(e.target.value) : undefined)
                }
              />
            </CCol>
            <CCol md={4}>
              <CFormLabel htmlFor="areaKm2">Area (km²)</CFormLabel>
              <CFormInput
                id="areaKm2"
                type="number"
                placeholder="e.g. 9833517"
                value={form.areaKm2 ?? ''}
                onChange={(e) =>
                  handleChange('areaKm2', e.target.value ? Number(e.target.value) : undefined)
                }
              />
            </CCol>
          </CRow>

          <div className="d-flex gap-2">
            <CButton color="primary" onClick={handleSave} disabled={saving}>
              {saving ? (
                <>
                  <CSpinner size="sm" className="me-1" />
                  Saving...
                </>
              ) : (
                <>
                  <CIcon icon={cilSave} className="me-1" />
                  {isNew ? 'Create Geography' : 'Save Changes'}
                </>
              )}
            </CButton>
            <CButton
              color="secondary"
              variant="outline"
              onClick={() => navigate('/administration/geographies')}
              disabled={saving}
            >
              Cancel
            </CButton>
          </div>
        </CCardBody>
      </CCard>
    </div>
  )
}

export default GeographyEdit
