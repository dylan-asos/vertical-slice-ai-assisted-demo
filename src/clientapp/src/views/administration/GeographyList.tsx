import React, { useState, useEffect } from 'react'
import { useNavigate } from 'react-router-dom'
import {
  CCard,
  CCardBody,
  CCardHeader,
  CTable,
  CTableBody,
  CTableDataCell,
  CTableHead,
  CTableHeaderCell,
  CTableRow,
  CSpinner,
  CAlert,
  CFormInput,
  CRow,
  CCol,
  CBadge,
  CPagination,
  CPaginationItem,
  CFormSelect,
  CButton,
} from '@coreui/react'
import CIcon from '@coreui/icons-react'
import { cilGlobeAlt, cilPen, cilPlus } from '@coreui/icons'
import { useVerticalSliceClient, getGeographies } from '../../api'
import type { GeographySummary, GetGeographiesParams } from '../../api'

const GeographyList = () => {
  useVerticalSliceClient()

  const navigate = useNavigate()
  const [geographies, setGeographies] = useState<GeographySummary[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)
  const [currentPage, setCurrentPage] = useState(1)
  const [totalPages, setTotalPages] = useState(1)
  const [totalItems, setTotalItems] = useState(0)
  const [pageSize, setPageSize] = useState(25)

  const [searchTerm, setSearchTerm] = useState('')
  const [regionFilter, setRegionFilter] = useState('')

  useEffect(() => {
    loadGeographies()
  }, [currentPage, pageSize, searchTerm, regionFilter])

  const loadGeographies = async () => {
    try {
      setLoading(true)
      setError(null)

      const params: GetGeographiesParams = {
        page: currentPage,
        pageSize: pageSize,
        searchTerm: searchTerm || undefined,
        region: regionFilter || undefined,
      }

      const response = await getGeographies(params)

      setGeographies(response.data.data ?? [])
      setTotalPages(response.data.pagination?.totalPages ?? 1)
      setTotalItems(response.data.pagination?.totalItems ?? 0)
    } catch (err) {
      console.error('Failed to load geographies:', err)
      setError('Failed to load geography records')
    } finally {
      setLoading(false)
    }
  }

  const handlePageChange = (page: number) => {
    setCurrentPage(page)
  }

  const handlePageSizeChange = (newPageSize: number) => {
    setPageSize(newPageSize)
    setCurrentPage(1)
  }

  const clearFilters = () => {
    setSearchTerm('')
    setRegionFilter('')
    setCurrentPage(1)
  }

  const formatPopulation = (population: number | null | undefined) => {
    if (!population) return '—'
    if (population >= 1_000_000_000) return `${(population / 1_000_000_000).toFixed(1)}B`
    if (population >= 1_000_000) return `${(population / 1_000_000).toFixed(1)}M`
    if (population >= 1_000) return `${(population / 1_000).toFixed(0)}K`
    return population.toLocaleString()
  }

  const formatArea = (area: number | null | undefined) => {
    if (!area) return '—'
    return `${area.toLocaleString()} km²`
  }

  const getRegionBadgeColor = (region: string | null | undefined) => {
    switch (region) {
      case 'Americas':
        return 'primary'
      case 'Europe':
        return 'info'
      case 'Asia':
        return 'warning'
      case 'Africa':
        return 'success'
      case 'Oceania':
        return 'secondary'
      default:
        return 'light'
    }
  }

  const renderPagination = () => {
    if (totalPages <= 1) return null

    const items = []
    const maxVisiblePages = 5
    let startPage = Math.max(1, currentPage - Math.floor(maxVisiblePages / 2))
    const endPage = Math.min(totalPages, startPage + maxVisiblePages - 1)

    if (endPage - startPage + 1 < maxVisiblePages) {
      startPage = Math.max(1, endPage - maxVisiblePages + 1)
    }

    items.push(
      <CPaginationItem
        key="prev"
        disabled={currentPage === 1}
        onClick={() => handlePageChange(currentPage - 1)}
      >
        Previous
      </CPaginationItem>,
    )

    for (let i = startPage; i <= endPage; i++) {
      items.push(
        <CPaginationItem key={i} active={i === currentPage} onClick={() => handlePageChange(i)}>
          {i}
        </CPaginationItem>,
      )
    }

    items.push(
      <CPaginationItem
        key="next"
        disabled={currentPage === totalPages}
        onClick={() => handlePageChange(currentPage + 1)}
      >
        Next
      </CPaginationItem>,
    )

    return <CPagination>{items}</CPagination>
  }

  if (loading && geographies.length === 0) {
    return (
      <CCard>
        <CCardBody className="text-center">
          <CSpinner size="lg" />
          <p className="mt-3">Loading geographies...</p>
        </CCardBody>
      </CCard>
    )
  }

  return (
    <div>
      <CCard>
        <CCardHeader>
          <div className="d-flex justify-content-between align-items-center">
            <div className="d-flex align-items-center">
              <CIcon icon={cilGlobeAlt} className="me-2" />
              <h5 className="mb-0">Geographies</h5>
            </div>
            <div className="d-flex align-items-center gap-2">
              <CBadge color="info" className="fs-6">
                {totalItems.toLocaleString()} records
              </CBadge>
              <CButton
                color="primary"
                size="sm"
                onClick={() => navigate('/administration/geographies/new')}
              >
                <CIcon icon={cilPlus} className="me-1" />
                Add Geography
              </CButton>
            </div>
          </div>
        </CCardHeader>
        <CCardBody>
          {error && (
            <CAlert color="danger" className="mb-3">
              {error}
            </CAlert>
          )}

          {/* Filters */}
          <CRow className="mb-3">
            <CCol md={4}>
              <CFormInput
                type="text"
                placeholder="Search by name, code, capital..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
              />
            </CCol>
            <CCol md={3}>
              <CFormSelect value={regionFilter} onChange={(e) => setRegionFilter(e.target.value)}>
                <option value="">All Regions</option>
                <option value="Americas">Americas</option>
                <option value="Europe">Europe</option>
                <option value="Asia">Asia</option>
                <option value="Africa">Africa</option>
                <option value="Oceania">Oceania</option>
              </CFormSelect>
            </CCol>
            <CCol md={2}>
              <CFormSelect
                value={pageSize}
                onChange={(e) => handlePageSizeChange(Number(e.target.value))}
              >
                <option value={25}>25 per page</option>
                <option value={50}>50 per page</option>
                <option value={100}>100 per page</option>
              </CFormSelect>
            </CCol>
            <CCol md={1}>
              <CButton color="secondary" variant="outline" onClick={clearFilters} className="w-100">
                Clear
              </CButton>
            </CCol>
          </CRow>

          {/* Table */}
          <CTable responsive striped hover>
            <CTableHead>
              <CTableRow>
                <CTableHeaderCell>Name</CTableHeaderCell>
                <CTableHeaderCell>Code</CTableHeaderCell>
                <CTableHeaderCell>Region</CTableHeaderCell>
                <CTableHeaderCell>Sub-Region</CTableHeaderCell>
                <CTableHeaderCell>Capital</CTableHeaderCell>
                <CTableHeaderCell>Population</CTableHeaderCell>
                <CTableHeaderCell>Area</CTableHeaderCell>
                <CTableHeaderCell>Actions</CTableHeaderCell>
              </CTableRow>
            </CTableHead>
            <CTableBody>
              {geographies.map((geo) => (
                <CTableRow key={geo.geographyId}>
                  <CTableDataCell>
                    <strong>{geo.name}</strong>
                  </CTableDataCell>
                  <CTableDataCell>
                    <CBadge color="secondary" className="font-monospace">
                      {geo.shortCode}
                    </CBadge>
                  </CTableDataCell>
                  <CTableDataCell>
                    {geo.region && (
                      <CBadge color={getRegionBadgeColor(geo.region)}>{geo.region}</CBadge>
                    )}
                  </CTableDataCell>
                  <CTableDataCell>
                    <small className="text-muted">{geo.subRegion ?? '—'}</small>
                  </CTableDataCell>
                  <CTableDataCell>{geo.capital ?? '—'}</CTableDataCell>
                  <CTableDataCell>{formatPopulation(geo.population)}</CTableDataCell>
                  <CTableDataCell>{formatArea(geo.areaKm2)}</CTableDataCell>
                  <CTableDataCell>
                    <CButton
                      color="primary"
                      variant="outline"
                      size="sm"
                      onClick={() => navigate(`/administration/geographies/${geo.geographyId}`)}
                    >
                      <CIcon icon={cilPen} className="me-1" />
                      Edit
                    </CButton>
                  </CTableDataCell>
                </CTableRow>
              ))}
            </CTableBody>
          </CTable>

          {/* Pagination */}
          <div className="d-flex justify-content-between align-items-center mt-3">
            <div>
              {totalItems > 0 && (
                <>
                  Showing {(currentPage - 1) * pageSize + 1} to{' '}
                  {Math.min(currentPage * pageSize, totalItems)} of {totalItems.toLocaleString()}{' '}
                  records
                </>
              )}
            </div>
            {renderPagination()}
          </div>
        </CCardBody>
      </CCard>
    </div>
  )
}

export default GeographyList
